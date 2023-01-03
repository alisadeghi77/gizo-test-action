namespace Gizo.Api.Contracts.UserProfile.Responses;

public record UserProfileResponse
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string EmailAddress { get; set; }
    public string Phone { get; set; }
}
