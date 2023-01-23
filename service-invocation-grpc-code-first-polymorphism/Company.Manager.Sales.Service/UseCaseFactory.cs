using Company.Framework;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Company.Manager.Sales.Service
{
    static class UseCaseFactory<C, R>
        where C : class
        where R : class
    {
        private readonly record struct Key(string Namespace, string Name);

        private delegate Task<R> UseCase(C criteria);

        private static readonly ConcurrentDictionary<Key, UseCase> Cache = new();

        static UseCase Resolve(Key key)
        {
            string typeName = key.Namespace.Replace("Interface", "Service") + ".UseCases";

            Type implementationType = Assembly.GetExecutingAssembly().GetType(typeName, true)!;            
            MethodInfo method = implementationType.GetMethod(key.Name) ?? throw new InvalidOperationException($"{implementationType.FullName} does not have a public method {key.Name}");
            
            Func<object, C, Task<R>> useCaseFunc = ReflectionUtil.CreateCovariantTaskDelegate<C, R>(method);

            Task<R> useCase(C criteria)
            {
                object instance = Activator.CreateInstance(implementationType)!;
                return Task.Run(() => useCaseFunc(instance, criteria));
            }

            return useCase;
        }

        public static Task<R> CallAsync(C criteria, [CallerMemberName] string name = "")
        {
            UseCase useCase = Cache.GetOrAdd(new(criteria.GetType().Namespace!, name), Resolve);

            return useCase(criteria);
        }
    }
}
