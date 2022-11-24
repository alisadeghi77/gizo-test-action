
namespace Gizo.Api.Contracts.Posts.Requests;

public class PostCommentCreateRequest
{
    [Required]
    public string Text { get;  set; }
    
}