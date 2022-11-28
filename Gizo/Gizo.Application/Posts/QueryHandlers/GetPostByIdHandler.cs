using Gizo.Domain.Aggregates.PostAggregate;
using Gizo.Application.Enums;
using Gizo.Application.Models;
using Gizo.Application.Posts.Queries;
using Gizo.Domain.Contracts.Repository;
using Gizo.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gizo.Application.Posts.QueryHandlers;

public class GetPostByIdHandler : IRequestHandler<GetPostByIdQuery, OperationResult<Post>>
{
    private readonly IRepository<Post> _postRepository;

    public GetPostByIdHandler(IRepository<Post> postRepository)
    {
        _postRepository = postRepository;
    }
    public async Task<OperationResult<Post>> Handle(GetPostByIdQuery request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<Post>();
        var post = await _postRepository
            .GetByIdAsync(request.PostId);

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