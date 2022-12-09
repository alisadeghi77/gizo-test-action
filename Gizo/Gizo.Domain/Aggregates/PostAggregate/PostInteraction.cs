using Gizo.Domain.Contracts.Base;

namespace Gizo.Domain.Aggregates.PostAggregate;

public class PostInteraction: BaseEntity<long>
{
    private PostInteraction()
    {

    }
    public long PostId { get; private set; }
    public InteractionType InteractionType { get; private set; }

    //Factories
    public static PostInteraction CreatePostInteraction(long postId, long userProfileId,
        InteractionType type)
    {
        return new PostInteraction
        {
            PostId = postId,
            InteractionType = type
        };
    }

}
