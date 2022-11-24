
namespace Gizo.Api.Contracts.Identity;

public class LoginRequest
{
    [EmailAddress]
    [Required]
    public string Username { get; set; }
    
    [Required]
    public string Password { get; set; }
}