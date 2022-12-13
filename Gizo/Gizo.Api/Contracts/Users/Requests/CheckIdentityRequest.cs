namespace Gizo.Api.Contracts.Users.Requests;

public class CheckIdentityRequest
{
    [Required]
    public string Username { get; set; }
}
