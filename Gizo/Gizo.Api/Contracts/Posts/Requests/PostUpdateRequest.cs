namespace Gizo.Api.Contracts.Posts.Requests;

public class PostUpdateRequest
{
    [Required]
    public string Text { get; set; }
}