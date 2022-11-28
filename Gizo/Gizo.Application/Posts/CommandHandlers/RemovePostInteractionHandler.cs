using Gizo.Domain.Aggregates.PostAggregate;
using Gizo.Application.Enums;
using Gizo.Application.Models;
using Gizo.Application.Posts.Commands;
using Gizo.Domain.Contracts.Repository;
using MediatR;

namespace Gizo.Application.Posts.CommandHandlers;

public class RemovePostInteractionHandler : IRequestHandler<RemovePostInteractionCommand, OperationResult<PostInteraction>>
{
    private readonly IRepository<Post> _postRepository;
    private readonly IRepository<PostInteraction> _interactionRepository;
    private readonly IUnitOfWork _uow;

    public RemovePostInteractionHandler(
        IRepository<Post> postRepository,
        IRepository<PostInteraction> interactionRepository,
        IUnitOfWork uow)
    {
        _postRepository = postRepository;
        _interactionRepository = interactionRepository;
        _uow = uow;
    }
    public async Task<OperationResult<PostInteraction>> Handle(RemovePostInteractionCommand request,
        CancellationToken cancellationToken)
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

            var interaction = await _interactionRepository
                .Get()
                .Filter(_ =>
                    _.Id == request.InteractionId
                    && _.PostId == request.PostId)
                .FirstAsync();

            if (interaction == null)
            {
                result.AddError(ErrorCode.NotFound, PostsErrorMessages.PostInteractionNotFound);
                return result;
            }

            if (interaction.UserProfileId != request.UserProfileId)
            {
                result.AddError(ErrorCode.InteractionRemovalNotAuthorized,
                    PostsErrorMessages.InteractionRemovalNotAuthorized);
                return result;
            }

            _interactionRepository.Delete(interaction);
            await _uow.SaveChangesAsync(cancellationToken);

            result.Data = interaction;
        }
        catch (Exception e)
        {
            result.AddUnknownError(e.Message);
        }

        return result;
    }
}