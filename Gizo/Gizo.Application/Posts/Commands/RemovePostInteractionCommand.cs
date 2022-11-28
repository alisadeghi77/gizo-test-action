using Gizo.Domain.Aggregates.PostAggregate;
using Gizo.Application.Models;
using MediatR;

namespace Gizo.Application.Posts.Commands;

public class RemovePostInteractionCommand : IRequest<OperationResult<PostInteraction>>
{
    public long PostId { get; set; }
    public long InteractionId { get; set; }
    public long UserProfileId { get; set; }
}