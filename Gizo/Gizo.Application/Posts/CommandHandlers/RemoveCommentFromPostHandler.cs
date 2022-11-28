using Gizo.Domain.Aggregates.PostAggregate;
using Gizo.Application.Enums;
using Gizo.Application.Models;
using Gizo.Application.Posts.Commands;
using Gizo.Domain.Contracts.Repository;
using MediatR;

namespace Gizo.Application.Posts.CommandHandlers;

public class RemoveCommentFromPostHandler 
    : IRequestHandler<RemoveCommentFromPostCommand, OperationResult<PostComment>>
{
    private readonly IRepository<Post> _postRepository;
    private readonly IRepository<PostComment> _commentRepository;
    private readonly IUnitOfWork _uow;

    public RemoveCommentFromPostHandler(
        IRepository<Post> postRepository,
        IRepository<PostComment> commentRepository,
        IUnitOfWork uow)
    {
        _postRepository = postRepository;
        _commentRepository = commentRepository;
        _uow = uow;
    }

    public async Task<OperationResult<PostComment>> Handle(RemoveCommentFromPostCommand request, 
        CancellationToken cancellationToken)
    {
        var result = new OperationResult<PostComment>();

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

        var comment = await _commentRepository
            .Get()
            .Filter(_ =>
                _.Id == request.CommentId
                && _.PostId == request.PostId)
            .FirstAsync();

        if (comment == null)
        {
            result.AddError(ErrorCode.NotFound, PostsErrorMessages.PostCommentNotFound);
            return result;
        }

        if (comment.UserProfileId != request.UserProfileId)
        {
            result.AddError(ErrorCode.CommentRemovalNotAuthorized, 
                PostsErrorMessages.CommentRemovalNotAuthorized);
            return result;
        }

        await _commentRepository.DeleteAsync(request.CommentId);
        await _uow.SaveChangesAsync(cancellationToken);

        result.Data = comment;
        return result;
    }
}