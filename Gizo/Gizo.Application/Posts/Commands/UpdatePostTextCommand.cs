using Gizo.Domain.Aggregates.PostAggregate;
using Gizo.Application.Models;
using MediatR;

namespace Gizo.Application.Posts.Commands;

public sealed record UpdatePostTextCommand(
    string NewText,
    long PostId,
    long UserProfileId
    ) : IRequest<OperationResult<Post>>
{ }
