using System.Reflection;

namespace Company.Framework
{
    public static class ReflectionUtil
    {
        public static Func<object, TParamBase, Task<TResultBase>> CreateCovariantTaskDelegate<TParamBase, TResultBase>(MethodInfo method)
        {
            if (!method.ReturnType.IsConstructedGenericType || method.ReturnType.GetGenericTypeDefinition() != typeof(Task<>))
            {
                throw new ArgumentException($"{method.Name} does not return a Task<TResult>");
            }

            string prop = nameof(Task<TResultBase>.Result);
            PropertyInfo result = method.ReturnType.GetProperty(prop)!;

            Func<object, TParamBase, Task> taskFunc = CreateDelegateUnknownTargetDowncastParamUpcastResult<TParamBase, Task>(method);
            Func<Task, TResultBase> resultFunc = CreateDelegateDowncastTarget<Task, TResultBase>(result.GetMethod!);

            return (instance, param) => taskFunc(instance, param).ContinueWith(resultFunc);
        }

        static Func<object, TParamBase, TResultBase> CreateDelegateUnknownTargetDowncastParamUpcastResult<TParamBase, TResultBase>(MethodInfo method)
        {
            MethodInfo genericHelper = typeof(ReflectionUtil).GetMethod(nameof(CreateDelegateUnknownTargetDowncastParamUpcastResultHelper), BindingFlags.Static | BindingFlags.NonPublic)!;

            MethodInfo constructedHelper = genericHelper.MakeGenericMethod(method.ReflectedType!, method.GetParameters()[0].ParameterType, method.ReturnType, typeof(TParamBase), typeof(TResultBase));

            return (Func<object, TParamBase, TResultBase>)constructedHelper.Invoke(null, new object[] { method })!;
        }

        static Func<object, TParamBase, TResultBase> CreateDelegateUnknownTargetDowncastParamUpcastResultHelper<TTarget, TParam, TResult, TParamBase, TResultBase>(MethodInfo method)
            where TParamBase : class
            where TParam : TParamBase
            where TResult : TResultBase
        {            
            Func<TTarget, TParam, TResult> func = method.CreateDelegate<Func<TTarget, TParam, TResult>>();

            return (target, param) => func((TTarget)target, (TParam)param);
        }

        static Func<TTargetBase, TResult> CreateDelegateDowncastTarget<TTargetBase, TResult>(MethodInfo method)
        {
            MethodInfo genericHelper = typeof(ReflectionUtil).GetMethod(nameof(CreateDelegateDowncastTargetHelper), BindingFlags.Static | BindingFlags.NonPublic)!;

            MethodInfo constructedHelper = genericHelper.MakeGenericMethod(method.ReflectedType!, method.ReturnType, typeof(TTargetBase));

            return (Func<TTargetBase, TResult>)constructedHelper.Invoke(null, new object[] { method })!;
        }

        static Func<TTargetBase, TResult> CreateDelegateDowncastTargetHelper<TTarget, TResult, TTargetBase>(MethodInfo method)
            where TTargetBase : class
            where TTarget : TTargetBase
        {            
            Func<TTarget, TResult> func = method.CreateDelegate<Func<TTarget, TResult>>();

            return target => func((TTarget)target);
        }
    }
}
