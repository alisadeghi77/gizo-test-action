namespace Gizo.Domain.Exceptions;

public class FileUploadedException : Exception
{
    public FileUploadedException(string message) : base(message)
    {
    }
}
