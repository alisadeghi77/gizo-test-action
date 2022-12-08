using Gizo.Domain.Aggregates.PostAggregate;
using Gizo.Application.Enums;
using Gizo.Application.Models;
using Gizo.Application.Posts.Queries;
using Gizo.Domain.Contracts.Repository;
using MediatR;

namespace Gizo.Application.Posts.QueryHandlers;

public class GetPostInteractionsHandler : IRequestHandler<GetPostInteractionsQuery, OperationResult<List<PostInteraction>>>
{
    private readonly IRepository<Post> _postRepository;
    private readonly IRepository<PostInteraction> _interactionRepository;

    public GetPostInteractionsHandler(
        IRepository<Post> postRepository,
        IRepository<PostInteraction> interactionRepository)
    {
        _postRepository = postRepository;
        _interactionRepository = interactionRepository;
    }

    public async Task<OperationResult<List<PostInteraction>>> Handle(GetPostInteractionsQuery request, 
        CancellationToken token)
    {
        var result = new OperationResult<List<PostInteraction>>();

        try
        {
            var postExists = await _postRepository
                .Get()
                .Filter(_ => _.Id == request.PostId)
                .AnyAsync();

            if (!postExists)
            {
                result.AddError(ErrorCode.NotFound, "Post not found");
                return result;
            }

            result.Data = await _interactionRepository
                .Get()
                .Filter(_ => _.PostId == request.PostId)
                .ToListAsync();

        }
        catch (Exception e)
        {
            result.AddUnknownError(e.Message);
        }

        return result;
    }
}