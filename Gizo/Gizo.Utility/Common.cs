namespace Gizo.Utility;

public static class Common
{
    public static bool NotNullAny<T>(this IEnumerable<T> enumerable)
    {
        return enumerable is not null && enumerable.Any();
    }

    public static bool NotNullAny<T>(this IEnumerable<T> enumerable, Func<T, bool> predict)
    {
        return enumerable is not null && enumerable.Any(predict);
    }
}
