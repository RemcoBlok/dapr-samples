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
        readonly record struct Key(string Namespace, string Name);

        delegate Task<R> UseCase(C criteria);

        static readonly ConcurrentDictionary<Key, UseCase> _Cache;

        static UseCaseFactory()
        {
            _Cache = new ConcurrentDictionary<Key, UseCase>();
        }

        static UseCase Resolve(Key key)
        {
            string typeName = key.Namespace.Replace("Interface", "Service") + ".UseCases";

            Assembly assembly = Assembly.GetExecutingAssembly();

            Type? implementationType = assembly.GetType(typeName);
            Debug.Assert(implementationType != null, $"{typeName} not found");

            MethodInfo? method = implementationType.GetMethod(key.Name);
            Debug.Assert(method != null, $"{key.Name} not found");

            Func<object, C, Task<R>> useCaseFunc = ReflectionUtil.CreateCovariantTaskDelegate<C, R>(method);

            UseCase useCase =
                (C criteria) =>
                {
                    object? instance = Activator.CreateInstance(implementationType);
                    Debug.Assert(instance != null);

                    Func<Task<R>> func =
                        () =>
                        {
                            Task<R> task = useCaseFunc(instance, criteria);
                            return task;
                        };

                    Task<R> task = Task.Run(func);
                    return task;
                };

            return useCase;
        }

        public static Task<R> CallAsync(C criteria, [CallerMemberName] string? name = null)
        {
            Type criteriaType = criteria.GetType();

            Debug.Assert(criteriaType.Namespace != null);
            Debug.Assert(name != null);

            Key key = new Key(criteriaType.Namespace, name);

            UseCase useCase = _Cache.GetOrAdd(key, Resolve);

            return useCase(criteria);
        }
    }
}