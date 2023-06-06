using System.Diagnostics;
using System.Reflection;

namespace Company.Framework
{
    public static class ReflectionUtil
    {
        public static Func<object, ParamBase, Task<ResultBase>> CreateCovariantTaskDelegate<ParamBase, ResultBase>(MethodInfo method)
            where ParamBase : class
            where ResultBase : class
        {
            Type taskOfResultType = typeof(Task<>);
            bool returnsTaskOfResult = method.ReturnType.GetGenericTypeDefinition() == taskOfResultType && method.ReturnType.GetTypeInfo().IsConstructedGenericType;
            Debug.Assert(returnsTaskOfResult, $"{method.Name} does not return a Task<TResult>");

            string resultPropertyName = nameof(Task<ResultBase>.Result);
            PropertyInfo? resultProperty = method.ReturnType.GetProperty(resultPropertyName);
            Debug.Assert(resultProperty != null);
            Debug.Assert(resultProperty.GetMethod != null);

            Func<object, ParamBase, Task> taskFunc = CreateDelegateUnknownTargetDowncastParamUpcastResult<ParamBase, Task>(method);
            Func<Task, ResultBase> resultFunc = CreateDelegateDowncastTarget<Task, ResultBase>(resultProperty.GetMethod);

            Func<object, ParamBase, Task<ResultBase>> func =
                (object instance, ParamBase param) =>
                {
                    Task task = taskFunc(instance, param);

                    TaskScheduler scheduler = TaskScheduler.Default;
                    Task<ResultBase> resultTask = task.ContinueWith(resultFunc, scheduler);
                    return resultTask;
                };

            return func;
        }

        static Func<object, ParamBase, ResultBase> CreateDelegateUnknownTargetDowncastParamUpcastResult<ParamBase, ResultBase>(MethodInfo method)
        {
            Type reflectionUtilType = typeof(ReflectionUtil);
            MethodInfo? genericHelper = reflectionUtilType
                .GetMethod(nameof(CreateDelegateUnknownTargetDowncastParamUpcastResultHelper), BindingFlags.Static | BindingFlags.NonPublic);
            Debug.Assert(genericHelper != null);

            ParameterInfo[] parameters = method.GetParameters();
            Debug.Assert(parameters.Length == 1);
            Debug.Assert(parameters[0].ParameterType.IsClass);

            Type paramBaseType = typeof(ParamBase);
            Type resultBaseType = typeof(ResultBase);

            Debug.Assert(method.ReflectedType != null);
            Debug.Assert(method.ReflectedType.IsClass);
            MethodInfo constructedHelper = genericHelper
                .MakeGenericMethod(method.ReflectedType, parameters[0].ParameterType, method.ReturnType, paramBaseType, resultBaseType);

            object[] arguments = new object[] { method };
            Func<object, ParamBase, ResultBase>? func = constructedHelper.Invoke(null, arguments) as Func<object, ParamBase, ResultBase>;
            Debug.Assert(func != null);
            return func;
        }

        static Func<object, ParamBase, ResultBase> CreateDelegateUnknownTargetDowncastParamUpcastResultHelper<Target, Param, Result, ParamBase, ResultBase>(MethodInfo method)
            where Param : class, ParamBase
            where Result : ResultBase
            where Target : class
        {
            Func<Target, Param, Result> func = method.CreateDelegate<Func<Target, Param, Result>>();

            Func<object, ParamBase, ResultBase> resultFunc =
                (object unknownTarget, ParamBase param) =>
                {
                    Target? target = unknownTarget as Target;
                    Debug.Assert(target != null);

                    Param? downcastParam = param as Param;
                    Debug.Assert(downcastParam != null);

                    ResultBase upcastResult = func(target, downcastParam);
                    return upcastResult;
                };

            return resultFunc;
        }

        static Func<TargetBase, Result> CreateDelegateDowncastTarget<TargetBase, Result>(MethodInfo method)
        {
            Type reflectionUtilType = typeof(ReflectionUtil);
            MethodInfo? genericHelper = reflectionUtilType.GetMethod(nameof(CreateDelegateDowncastTargetHelper), BindingFlags.Static | BindingFlags.NonPublic);
            Debug.Assert(genericHelper != null);

            Type targetBaseType = typeof(TargetBase);

            Debug.Assert(method.ReflectedType != null);
            Debug.Assert(method.ReflectedType.IsClass);
            MethodInfo constructedHelper = genericHelper.MakeGenericMethod(method.ReflectedType, method.ReturnType, targetBaseType);

            object[] arguments = new object[] { method };
            Func<TargetBase, Result>? func = constructedHelper.Invoke(null, arguments) as Func<TargetBase, Result>;
            Debug.Assert(func != null);
            return func;
        }

        static Func<TargetBase, Result> CreateDelegateDowncastTargetHelper<Target, Result, TargetBase>(MethodInfo method)
            where Target : class, TargetBase
        {
            Func<Target, Result> func = method.CreateDelegate<Func<Target, Result>>();

            Func<TargetBase, Result> resultFunc =
                (TargetBase target) =>
                {
                    Target? downcastTarget = target as Target;
                    Debug.Assert(downcastTarget != null);

                    Result result = func(downcastTarget);
                    return result;
                };

            return resultFunc;
        }
    }
}
