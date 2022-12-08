using Gizo.Domain.Aggregates.PostAggregate;
using Gizo.Domain.Exceptions;
using Gizo.Application.Enums;
using Gizo.Application.Models;
using Gizo.Application.Posts.Commands;
using Gizo.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gizo.Application.Posts.CommandHandlers;

public class UpdatePostTextHandler : IRequestHandler<UpdatePostTextCommand, OperationResult<Post>>
{
    private readonly DataContext _ctx;

    public UpdatePostTextHandler(DataContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<OperationResult<Post>> Handle(UpdatePostTextCommand request, CancellationToken token)
    {
        var result = new OperationResult<Post>();

        try
        {
            var post = await _ctx.Posts.FirstOrDefaultAsync(p => p.Id == request.PostId, cancellationToken: token);

            if (post is null)
            {
                result.AddError(ErrorCode.NotFound, 
                    string.Format(PostsErrorMessages.PostNotFound, request.PostId));
                return result;
            }

            if (post.UserProfileId != request.UserProfileId)
            {
                result.AddError(ErrorCode.PostUpdateNotPossible, PostsErrorMessages.PostUpdateNotPossible);
                return result;
            }

            post.UpdatePostText(request.NewText);

            await _ctx.SaveChangesAsync(token);

            result.Data = post;
        }

        catch (PostNotValidException e)
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