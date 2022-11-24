namespace Gizo.Api.Contracts.Posts.Requests;

public class PostInteractionCreateRequest
{
    [Required]
    public InteractionType Type { get; set; }
}