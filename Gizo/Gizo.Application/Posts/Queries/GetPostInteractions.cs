using Gizo.Domain.Aggregates.PostAggregate;
using Gizo.Application.Models;
using MediatR;

namespace Gizo.Application.Posts.Queries;

public class GetPostInteractions : IRequest<OperationResult<List<PostInteraction>>>
{
    public Guid PostId { get; set; }
}