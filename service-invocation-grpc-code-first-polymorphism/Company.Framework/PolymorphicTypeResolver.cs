using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

namespace Company.Framework
{
    public class PolymorphicTypeResolver : DefaultJsonTypeInfoResolver
    {
        public override JsonTypeInfo GetTypeInfo(Type type, JsonSerializerOptions options)
        {
            JsonTypeInfo typeInfo = base.GetTypeInfo(type, options);

            if (type.IsAbstract && type.IsClass)
            {
                List<JsonDerivedType> derivedTypes = typeInfo.Type.Assembly.GetTypes()
                    .Where(t => !t.IsAbstract && t.IsClass && typeInfo.Type.IsAssignableFrom(t))
                    .Select(t => new JsonDerivedType(t, t.FullName!))
                    .ToList();

                if (derivedTypes.Any())
                {
                    typeInfo.PolymorphismOptions = new();

                    foreach (JsonDerivedType derivedType in derivedTypes)
                    {
                        typeInfo.PolymorphismOptions.DerivedTypes.Add(derivedType);
                    }
                }
            }
            // workaround for https://github.com/dotnet/aspnetcore/issues/44852
            // a pr with a fix was merged in https://github.com/dotnet/aspnetcore/pull/45405
            // but won't arrive until aspnetcore 8.0
            else if (!type.IsAbstract && type.IsClass && type.BaseType != null &&
                type.BaseType.IsAbstract && type.BaseType.IsClass)
            {
                typeInfo.PolymorphismOptions = new()
                {
                    DerivedTypes =
                    {
                        new(type, type.FullName!)
                    }
                };
            }

            return typeInfo;
        }
    }
}
