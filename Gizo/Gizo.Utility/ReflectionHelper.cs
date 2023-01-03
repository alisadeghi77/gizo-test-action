using System.Collections;

namespace Gizo.Utility;

public static class ReflectionHelper
{
    public static bool IsEnumerable(this Type type)
    {
        return type != null && type != typeof(string) && typeof(IEnumerable).IsAssignableFrom(type);
    }

    public static bool IsAssignableToGenericType(this Type givenType, Type genericType)
    {
        var interfaceTypes = givenType.GetInterfaces();

        foreach (var it in interfaceTypes)
        {
            if (it.IsGenericType && it.GetGenericTypeDefinition() == genericType)
                return true;
        }

        if (givenType.IsGenericType && givenType.GetGenericTypeDefinition() == genericType)
            return true;

        Type? baseType = givenType.BaseType;
        if (baseType == null) return false;

        return IsAssignableToGenericType(baseType, genericType);
    }
}
