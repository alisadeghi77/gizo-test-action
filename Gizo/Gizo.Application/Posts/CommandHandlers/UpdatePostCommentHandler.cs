using Gizo.Domain.Aggregates.PostAggregate;
using Gizo.Application.Enums;
using Gizo.Application.Models;
using Gizo.Application.Posts.Commands;
using Gizo.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gizo.Application.Posts.CommandHandlers;

public class UpdatePostCommentHandler 
    : IRequestHandler<UpdatePostCommentCommand, OperationResult<PostComment>>
{
    private readonly DataContext _ctx;
    private readonly OperationResult<PostComment> _result;

    public UpdatePostCommentHandler(DataContext ctx)
    {
        _ctx = ctx;
        _result = new OperationResult<PostComment>();
    }
    
    public async Task<OperationResult<PostComment>> Handle(UpdatePostCommentCommand request, CancellationToken token)
    {
        var post = await _ctx.Posts
            .Include(p => p.Comments)
            .FirstOrDefaultAsync(p => p.Id == request.PostId, token);

        if (post == null)
        {
            _result.AddError(ErrorCode.NotFound, PostsErrorMessages.PostNotFound);
            return _result;
        }

        var comment = post.Comments
            .FirstOrDefault(c => c.Id == request.CommentId);
        if (comment == null)
        {
            _result.AddError(ErrorCode.NotFound, PostsErrorMessages.PostCommentNotFound);
            return _result;
        }
        
        comment.UpdateCommentText(request.UpdatedText);
        _ctx.Posts.Update(post);
        await _ctx.SaveChangesAsync(token);
        
        return _result;
    }
}