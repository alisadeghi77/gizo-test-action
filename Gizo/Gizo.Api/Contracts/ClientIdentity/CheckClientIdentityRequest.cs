namespace Gizo.Api.Contracts.ClientIdentity;

public class CheckClientIdentityRequest
{
    [Required]
    public string Username { get; set; }
}
