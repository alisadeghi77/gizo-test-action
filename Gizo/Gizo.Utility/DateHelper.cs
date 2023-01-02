namespace Gizo.Utility;

public static class DateHelper
{
    public static string ToStandardDateTime(this DateTime dateTime)
    {
        return dateTime.ToString("yyyy-MM-dd-HH-mm-ss.fff");
    }

    public static string ToStandardDate(this DateTime dateTime)
    {
        return dateTime.ToString("yyyy-MM-dd");
    }
}
