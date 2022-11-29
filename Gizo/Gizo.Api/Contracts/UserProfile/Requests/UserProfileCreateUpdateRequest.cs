namespace Gizo.Api.Contracts.UserProfile.Requests;

public record UserProfileCreateUpdateRequest
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
    public string EmailAddress { get; set; }

    public string Phone { get; set; }

    [Required]
    public DateTime DateOfBirth { get; set; }
    public string CurrentCity { get; set; }
}
