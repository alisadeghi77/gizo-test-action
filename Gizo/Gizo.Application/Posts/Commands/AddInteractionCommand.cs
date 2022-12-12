using Gizo.Domain.Aggregates.PostAggregate;
using Gizo.Application.Models;
using MediatR;

namespace Gizo.Application.Posts.Commands;

public sealed record AddInteractionCommand(
    long PostId,
    long UserProfileId,
    InteractionType Type
    ) : IRequest<OperationResult<PostInteraction>>
{ }
