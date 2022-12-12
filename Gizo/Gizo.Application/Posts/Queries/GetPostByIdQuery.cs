using Gizo.Domain.Aggregates.PostAggregate;
using Gizo.Application.Models;
using MediatR;

namespace Gizo.Application.Posts.Queries;

public sealed record GetPostByIdQuery(
    long PostId
    ) : IRequest<OperationResult<Post>>
{ }
