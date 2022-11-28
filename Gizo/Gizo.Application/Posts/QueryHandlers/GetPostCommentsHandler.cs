using Gizo.Domain.Aggregates.PostAggregate;
using Gizo.Application.Enums;
using Gizo.Application.Models;
using Gizo.Application.Posts.Queries;
using Gizo.Domain.Contracts.Repository;
using MediatR;

namespace Gizo.Application.Posts.QueryHandlers;

public class GetPostCommentsHandler : IRequestHandler<GetPostCommentsQuery, OperationResult<List<PostComment>>>
{
    private readonly IRepository<Post> _postRepository;
    private readonly IRepository<PostComment> _commentRepository;

    public GetPostCommentsHandler(
        IRepository<Post> postRepository, 
        IRepository<PostComment> commentRepository)
    {
        _postRepository = postRepository;
        _commentRepository = commentRepository;
    }

    public async Task<OperationResult<List<PostComment>>> Handle(GetPostCommentsQuery request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<List<PostComment>>();
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

            result.Data = await _commentRepository
                 .Get()
                 .Filter(_ => _.Id == request.PostId)
                 .ToListAsync();
        }
        catch (Exception e)
        {
            result.AddUnknownError(e.Message);
        }
        return result;
    }
}