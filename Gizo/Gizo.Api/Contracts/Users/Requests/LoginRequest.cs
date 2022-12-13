
namespace Gizo.Api.Contracts.Users.Requests;

public class LoginRequest
{
    [EmailAddress]
    [Required]
    public string Username { get; set; }
    
    [Required]
    public string Password { get; set; }
}