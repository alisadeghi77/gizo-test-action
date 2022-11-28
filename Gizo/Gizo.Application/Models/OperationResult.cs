using Gizo.Application.Enums;

namespace Gizo.Application.Models;

public class OperationResult<T>
{
    public T Data { get; set; }
    public bool IsError {get; private set;}

    private List<Error> _errors = new();
    public IReadOnlyCollection<Error> Errors => _errors;

    /// <summary>
    /// Adds an error to the Error list and sets the IsError flag to true
    /// </summary>
    /// <param name="code"></param>
    /// <param name="message"></param>
    public void AddError(ErrorCode code, string message)
    {
        HandleError(code, message);
    }

    /// <summary>
    /// Adds a default error to the Error list with the error code UnknownError
    /// </summary>
    /// <param name="message"></param>
    public void AddUnknownError(string message)
    {
        HandleError(ErrorCode.UnknownError, message);
    }

    /// <summary>
    /// Sets the IsError flag to default (false)
    /// </summary>
    public void ResetIsErrorFlag()
    {
        IsError = false;
    }

    private void HandleError(ErrorCode code, string message)
    {
        _errors.Add(new Error {Code = code, Message = message});
        IsError = true;
    }
}
