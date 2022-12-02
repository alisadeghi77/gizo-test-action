namespace Gizo.Api.Contracts.ClientIdentity;

public class VerifyClientIdentityRequest
{
    [Required]
    public string Username { get; set; }

    [Required]
    public string VerifyCode { get; set; }
}

