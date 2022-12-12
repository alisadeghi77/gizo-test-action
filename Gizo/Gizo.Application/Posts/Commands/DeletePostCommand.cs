using Gizo.Domain.Aggregates.PostAggregate;
using Gizo.Application.Models;
using MediatR;

namespace Gizo.Application.Posts.Commands;

public sealed record DeletePostCommand(
    long PostId,
    long UserProfileId
    ) : IRequest<OperationResult<Post>>
{ }
