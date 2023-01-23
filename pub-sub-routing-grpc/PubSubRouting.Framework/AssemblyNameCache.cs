using System.Collections.Concurrent;
using System.Reflection;

namespace PubSubRouting.Framework
{
    public static class AssemblyNameCache
    {
        private static readonly ConcurrentDictionary<Assembly, AssemblyName> AssemblyToNameCache = new();

        public static AssemblyName GetName(Assembly assembly)
        {
            return AssemblyToNameCache.GetOrAdd(assembly, asm => asm.GetName());
        }
    }
}
