namespace Gizo.Api.Contracts.Posts.Requests;

public class PostCommentUpdateRequest
{
    [Required]
    public string Text { get;  set; }
}