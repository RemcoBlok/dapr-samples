using System.Diagnostics;
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
                Type[] assemblyTypes = typeInfo.Type.Assembly.GetTypes();

                Func<Type, bool> isDerivedTypePredicate =
                    (Type t) =>
                    {
                        bool isDerivedType = t.IsAbstract == false && t.IsClass && typeInfo.Type.IsAssignableFrom(t);
                        return isDerivedType;
                    };

                Func<Type, JsonDerivedType> derivedTypeSelector =
                    (Type t) =>
                    {
                        Debug.Assert(t.FullName != null);
                        JsonDerivedType derivedType = new JsonDerivedType(t, t.FullName);
                        return derivedType;
                    };

                List<JsonDerivedType> derivedTypes = assemblyTypes
                    .Where(isDerivedTypePredicate)
                    .Select(derivedTypeSelector)
                    .ToList();

                if (derivedTypes.Any() == true)
                {
                    typeInfo.PolymorphismOptions = new JsonPolymorphismOptions();

                    foreach (JsonDerivedType derivedType in derivedTypes)
                    {
                        typeInfo.PolymorphismOptions.DerivedTypes.Add(derivedType);
                    }
                }
            }
            // workaround for https://github.com/dotnet/aspnetcore/issues/44852
            // a pr with a fix was merged in https://github.com/dotnet/aspnetcore/pull/45405
            // but won't arrive until aspnetcore 8.0
            else if (type.IsAbstract == false && type.IsClass &&
                type.BaseType != null && type.BaseType.IsAbstract && type.BaseType.IsClass)
            {
                Debug.Assert(type.FullName != null);
                JsonDerivedType derivedType = new JsonDerivedType(type, type.FullName);

                typeInfo.PolymorphismOptions = new JsonPolymorphismOptions();
                typeInfo.PolymorphismOptions.DerivedTypes.Add(derivedType);
            }

            return typeInfo;
        }
    }
}
