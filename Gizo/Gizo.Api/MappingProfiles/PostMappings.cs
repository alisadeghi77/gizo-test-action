using PostInteraction = Gizo.Domain.Aggregates.PostAggregate.PostInteraction;

namespace Gizo.Api.MappingProfiles;

public class PostMappings : Profile
{
    public PostMappings()
    {
        CreateMap<Post, PostResponse>();
        CreateMap<PostComment, PostCommentResponse>();
        CreateMap<PostInteraction, Gizo.Api.Contracts.Posts.Responses.PostInteraction>()
            .ForMember(dest 
                => dest.Type, opt 
                => opt.MapFrom(src 
                => src.InteractionType.ToString()))
            .ForMember(dest => dest.Author, opt 
            => opt.MapFrom(src => src.UserProfile));
    }
}