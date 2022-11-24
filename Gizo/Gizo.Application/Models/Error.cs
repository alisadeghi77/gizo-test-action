using Gizo.Application.Enums;

namespace Gizo.Application.Models;

public class Error
{
    public ErrorCode Code { get; set; }
    public string Message { get; set; }
}