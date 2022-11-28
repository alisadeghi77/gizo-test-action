using Gizo.Domain.Aggregates.PostAggregate;
using Gizo.Application.Models;
using MediatR;

namespace Gizo.Application.Posts.Commands;

public class AddInteractionCommand : IRequest<OperationResult<PostInteraction>>
{
    public long PostId { get; set; }
    public long UserProfileId { get; set; }
    public InteractionType Type { get; set; }
}