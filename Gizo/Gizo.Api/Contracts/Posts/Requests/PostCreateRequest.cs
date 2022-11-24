namespace Gizo.Api.Contracts.Posts.Requests;

public class PostCreateRequest
{
    [Required]
    public string TextContent { get; set; }
}