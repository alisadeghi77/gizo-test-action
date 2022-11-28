using Gizo.Domain.Aggregates.PostAggregate;
using Gizo.Application.Models;
using MediatR;

namespace Gizo.Application.Posts.Queries;

public class GetPostCommentsQuery : IRequest<OperationResult<List<PostComment>>>
{
    public long PostId { get; set; }
}