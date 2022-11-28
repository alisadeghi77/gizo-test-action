using Gizo.Domain.Aggregates.PostAggregate;
using Gizo.Application.Models;
using MediatR;

namespace Gizo.Application.Posts.Commands;

public class DeletePostCommand : IRequest<OperationResult<Post>>
{
    public long PostId { get; set; }
    public long UserProfileId { get; set; }
}