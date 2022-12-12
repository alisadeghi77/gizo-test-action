using Gizo.Domain.Aggregates.PostAggregate;
using Gizo.Application.Models;
using MediatR;

namespace Gizo.Application.Posts.Commands;

public sealed record UpdatePostCommentCommand(
    long UserProfileId,
    long PostId,
    long CommentId,
    string UpdatedText
    ) : IRequest<OperationResult<PostComment>>
{ }
