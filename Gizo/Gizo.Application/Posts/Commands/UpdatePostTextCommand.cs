using Gizo.Domain.Aggregates.PostAggregate;
using Gizo.Application.Models;
using MediatR;

namespace Gizo.Application.Posts.Commands;

public class UpdatePostTextCommand : IRequest<OperationResult<Post>>
{
    public string NewText { get; set; }
    public Guid PostId { get; set; }
    public Guid UserProfileId { get; set; }
}