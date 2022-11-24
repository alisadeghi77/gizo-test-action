using Gizo.Domain.Aggregates.PostAggregate;
using Gizo.Application.Enums;
using Gizo.Application.Models;
using Gizo.Application.Posts.Queries;
using Gizo.Dal;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gizo.Application.Posts.QueryHandlers;

public class GetAllPostsHandler : IRequestHandler<GetAllPosts, OperationResult<List<Post>>>
{
    private readonly DataContext _ctx;
    public GetAllPostsHandler(DataContext ctx)
    {
        _ctx = ctx;
    }
    public async Task<OperationResult<List<Post>>> Handle(GetAllPosts request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<List<Post>>();
        try
        {
            var posts = await _ctx.Posts.ToListAsync();
            result.Payload = posts;
        }
        catch (Exception e)
        {
            result.AddUnknownError(e.Message);
        }

        return result;
    }
}