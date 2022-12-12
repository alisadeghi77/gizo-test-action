using Gizo.Domain.Aggregates.PostAggregate;
using Gizo.Application.Models;
using MediatR;

namespace Gizo.Application.Posts.Commands;


public sealed record RemoveCommentFromPostCommand(
    long UserProfileId,
    long PostId,
    long CommentId
    ) : IRequest<OperationResult<PostComment>>
{ }