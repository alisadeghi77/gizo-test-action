using Gizo.Domain.Aggregates.PostAggregate;
using Gizo.Application.Enums;
using Gizo.Application.Models;
using Gizo.Application.Posts.Queries;
using Gizo.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gizo.Application.Posts.QueryHandlers;

public class GetPostInteractionsHandler : IRequestHandler<GetPostInteractions, OperationResult<List<PostInteraction>>>
{
    private readonly DataContext _ctx;

    public GetPostInteractionsHandler(DataContext ctx)
    {
        _ctx = ctx;
    }
    
    public async Task<OperationResult<List<PostInteraction>>> Handle(GetPostInteractions request, 
        CancellationToken cancellationToken)
    {
        var result = new OperationResult<List<PostInteraction>>();

        try
        {
            var post = await _ctx.Posts
                .Include(p => p.Interactions)
                .ThenInclude(i => i.UserProfile)
                .FirstOrDefaultAsync(p => p.PostId == request.PostId, cancellationToken);

            if (post == null)
            {
                result.AddError(ErrorCode.NotFound, PostsErrorMessages.PostNotFound);
                return result;
            }
            
            result.Data = post.Interactions.ToList();

        }
        catch (Exception e)
        {
            result.AddUnknownError(e.Message);
        }

        return result;
    }
}