using Gizo.Domain.Aggregates.PostAggregate;
using Gizo.Application.Models;
using MediatR;

namespace Gizo.Application.Posts.Queries;

public sealed record GetAllPostsQuery() : IRequest<OperationResult<List<Post>>>
{ }
