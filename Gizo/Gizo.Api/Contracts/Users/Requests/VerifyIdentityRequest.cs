namespace Gizo.Api.Contracts.Users.Requests;

public class VerifyIdentityRequest
{
    [Required]
    public string Username { get; set; }

    [Required]
    public string VerifyCode { get; set; }
}
