using Gizo.Domain.Aggregates.PostAggregate;
using Gizo.Application.Enums;
using Gizo.Application.Models;
using Gizo.Application.Posts.Commands;
using Gizo.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gizo.Application.Posts.CommandHandlers;

public class DeletePostHandler : IRequestHandler<DeletePostCommand, OperationResult<Post>>
{
    private readonly DataContext _ctx;

    public DeletePostHandler(DataContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<OperationResult<Post>> Handle(DeletePostCommand request, CancellationToken token)
    {
        var result = new OperationResult<Post>();
        try
        {
            var post = await _ctx.Posts.FirstOrDefaultAsync(p => p.Id == request.PostId,
                cancellationToken: token);

            if (post is null)
            {
                result.AddError(ErrorCode.NotFound,
                    string.Format(PostsErrorMessages.PostNotFound, request.PostId));

                return result;
            }


            _ctx.Posts.Remove(post);
            await _ctx.SaveChangesAsync(token);

            result.Data = post;
        }
        catch (Exception e)
        {
            result.AddUnknownError(e.Message);
        }

        return result;
    }
}