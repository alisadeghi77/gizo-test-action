using Gizo.Domain.Aggregates.PostAggregate;
using Gizo.Application.Models;
using MediatR;

namespace Gizo.Application.Posts.Queries;

public class GetPostInteractionsQuery : IRequest<OperationResult<List<PostInteraction>>>
{
    public long PostId { get; set; }
}