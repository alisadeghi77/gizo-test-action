using Gizo.Domain.Aggregates.PostAggregate;
using Gizo.Application.Models;
using Gizo.Application.Posts.Queries;
using Gizo.Domain.Contracts.Repository;
using MediatR;

namespace Gizo.Application.Posts.QueryHandlers;

public class GetAllPostsHandler : IRequestHandler<GetAllPostsQuery, OperationResult<List<Post>>>
{
    private readonly IRepository<Post> _postRepository;

    public GetAllPostsHandler(IRepository<Post> postRepository)
    {
        _postRepository = postRepository;
    }
    public async Task<OperationResult<List<Post>>> Handle(GetAllPostsQuery request, CancellationToken token)
    {
        var result = new OperationResult<List<Post>>();
        try
        {
            result.Data = await _postRepository
                .Get()
                .ToListAsync();
        }
        catch (Exception e)
        {
            result.AddUnknownError(e.Message);
        }

        return result;
    }
}