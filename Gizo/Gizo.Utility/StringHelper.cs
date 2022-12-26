namespace Gizo.Utility;

public static class StringHelper
{
    public static string ToStandardType(this string type)
    {
        return $".{type.Split('/').Last()}";
    }
}
