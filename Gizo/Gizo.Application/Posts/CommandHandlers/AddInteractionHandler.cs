using Gizo.Domain.Aggregates.PostAggregate;
using Gizo.Application.Enums;
using Gizo.Application.Models;
using Gizo.Application.Posts.Commands;
using Gizo.Domain.Contracts.Repository;
using MediatR;

namespace Gizo.Application.Posts.CommandHandlers;

public class AddInteractionHandler : IRequestHandler<AddInteractionCommand, OperationResult<PostInteraction>>
{
    private readonly IRepository<Post> _postRepository;
    private readonly IRepository<PostInteraction> _interactionRepository;
    private readonly IUnitOfWork _uow;

    public AddInteractionHandler(
        IRepository<Post> postRepository,
        IRepository<PostInteraction> interactionRepository,
        IUnitOfWork uow)
    {
        _postRepository = postRepository;
        _interactionRepository = interactionRepository;
        _uow = uow;
    }

    public async Task<OperationResult<PostInteraction>> Handle(AddInteractionCommand request, 
        CancellationToken token)
    {
        var result = new OperationResult<PostInteraction>();
        try
        {
            var postExists = await _postRepository
                .Get()
                .Filter(_ => _.Id == request.PostId)
                .AnyAsync();

            if (!postExists)
            {
                result.AddError(ErrorCode.NotFound,
                    string.Format(PostsErrorMessages.PostNotFound, request.PostId));
                return result;
            }

            var interaction = PostInteraction.CreatePostInteraction(request.PostId, request.UserProfileId,
                request.Type);

            await _interactionRepository.InsertAsync(interaction);

            await _uow.SaveChangesAsync(token);

            result.Data = interaction;

        }
        catch (Exception e)
        {
            result.AddUnknownError(e.Message);
        }

        return result;
    }
}