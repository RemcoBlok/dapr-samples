using System.Reflection;

namespace PubSubRouting.Framework
{
    public static class TypeExtensions
    {
        public static string GetSimpleAssemblyQualifiedName(this Type type)
        {
            return Assembly.CreateQualifiedName(AssemblyNameCache.GetName(type.Assembly).Name, type.FullName);
        }
    }
}
