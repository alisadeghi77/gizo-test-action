using Gizo.Domain.Aggregates.PostAggregate;
using Gizo.Application.Models;
using MediatR;

namespace Gizo.Application.Posts.Commands;

public class AddPostCommentCommand : IRequest<OperationResult<PostComment>>
{
    public long PostId { get; set; }
    public long UserProfileId { get; set; }
    public string CommentText { get; set; }
}