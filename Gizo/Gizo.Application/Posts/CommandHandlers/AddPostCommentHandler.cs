using Gizo.Domain.Aggregates.PostAggregate;
using Gizo.Domain.Exceptions;
using Gizo.Application.Enums;
using Gizo.Application.Models;
using Gizo.Application.Posts.Commands;
using Gizo.Domain.Contracts.Repository;
using MediatR;

namespace Gizo.Application.Posts.CommandHandlers;

public class AddPostCommentHandler : IRequestHandler<AddPostCommentCommand, OperationResult<PostComment>>
{
    private readonly IRepository<Post> _postRepository;
    private readonly IRepository<PostComment> _commentRepository;
    private readonly IUnitOfWork _uow;

    public AddPostCommentHandler(
        IRepository<Post> postRepository,
        IRepository<PostComment> commentRepository,
        IUnitOfWork uow)
    {
        _postRepository = postRepository;
        _commentRepository = commentRepository;
        _uow = uow;
    }
    public async Task<OperationResult<PostComment>> Handle(AddPostCommentCommand request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<PostComment>();

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

            var comment = PostComment.CreatePostComment(request.PostId, request.CommentText, request.UserProfileId);

            await _commentRepository.InsertAsync(comment);

            await _uow.SaveChangesAsync(cancellationToken);

            result.Data = comment;
        }

        catch (PostCommentNotValidException e)
        {
            e.ValidationErrors.ForEach(er => result.AddError(ErrorCode.ValidationError, er));
        }

        catch (Exception e)
        {
            result.AddUnknownError(e.Message);
        }

        return result;
    }
}