using Gizo.Domain.Aggregates.PostAggregate;
using Gizo.Application.Enums;
using Gizo.Application.Models;
using Gizo.Application.Posts.Queries;
using Gizo.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gizo.Application.Posts.QueryHandlers;

public class GetPostByIdHandler : IRequestHandler<GetPostByIdQuery, OperationResult<Post>>
{
    private readonly DataContext _ctx;
    public GetPostByIdHandler(DataContext ctx)
    {
        _ctx = ctx;
    }
    public async Task<OperationResult<Post>> Handle(GetPostByIdQuery request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<Post>();
        var post = await _ctx.Posts
            .FirstOrDefaultAsync(p => p.PostId == request.PostId);
            
        if (post is null)
        {
            result.AddError(ErrorCode.NotFound, 
                string.Format(PostsErrorMessages.PostNotFound, request.PostId));
            return result;
        }

        result.Data = post;
        return result;
    }
}