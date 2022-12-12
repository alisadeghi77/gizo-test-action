using Gizo.Domain.Aggregates.PostAggregate;
using Gizo.Application.Models;
using MediatR;

namespace Gizo.Application.Posts.Commands;

public sealed record AddPostCommentCommand(
    long PostId,
    long UserProfileId,
    string CommentText
    ) : IRequest<OperationResult<PostComment>>
{ }
