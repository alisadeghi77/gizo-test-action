using Gizo.Domain.Aggregates.UserProfileAggregate;
using Gizo.Domain.Contracts.Base;

namespace Gizo.Domain.Aggregates.PostAggregate;

public class PostInteraction: BaseEntity<long>
{
    private PostInteraction()
    {

    }
    public long PostId { get; private set; }
    public long? UserProfileId { get; private set; }
    public UserProfile UserProfile { get; private set; }
    public InteractionType InteractionType { get; private set; }

    //Factories
    public static PostInteraction CreatePostInteraction(long postId, long userProfileId,
        InteractionType type)
    {
        return new PostInteraction
        {
            PostId = postId,
            UserProfileId = userProfileId,
            InteractionType = type
        };
    }

}
