using Gizo.Domain.Aggregates.PostAggregate;
using Gizo.Domain.Exceptions;
using Gizo.Application.Enums;
using Gizo.Application.Models;
using Gizo.Application.Posts.Commands;
using Gizo.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gizo.Application.Posts.CommandHandlers;

public class AddPostCommentHandler : IRequestHandler<AddPostComment, OperationResult<PostComment>>
{
    private readonly DataContext _ctx;

    public AddPostCommentHandler(DataContext ctx)
    {
        _ctx = ctx;
    }
    public async Task<OperationResult<PostComment>> Handle(AddPostComment request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<PostComment>();

        try
        {
            var post = await _ctx.Posts.FirstOrDefaultAsync(p => p.PostId == request.PostId,
                cancellationToken: cancellationToken);
            if (post is null)
            {
                result.AddError(ErrorCode.NotFound,
                    string.Format(PostsErrorMessages.PostNotFound, request.PostId));
                return result;
            }

            var comment = PostComment.CreatePostComment(request.PostId, request.CommentText, request.UserProfileId);
            
            post.AddPostComment(comment);

            _ctx.Posts.Update(post);
            await _ctx.SaveChangesAsync(cancellationToken);

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