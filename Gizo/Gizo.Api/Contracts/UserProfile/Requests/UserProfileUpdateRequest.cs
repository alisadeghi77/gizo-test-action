namespace Gizo.Api.Contracts.UserProfile.Requests;

public record UserProfileUpdateRequest
{
    [Required]
    [MinLength(3)]
    [MaxLength(50)]
    public string FirstName { get; set; }

    [Required]
    [MinLength(3)]
    [MaxLength(50)]
    public string LastName { get; set; }

    [EmailAddress]
    [Required]
    public string Email { get; set; }
}
