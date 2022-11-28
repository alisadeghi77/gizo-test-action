using Gizo.Domain.Aggregates.PostAggregate;
using Gizo.Application.Models;
using MediatR;

namespace Gizo.Application.Posts.Commands;

public class UpdatePostTextCommand : IRequest<OperationResult<Post>>
{
    public string NewText { get; set; }
    public long PostId { get; set; }
    public long UserProfileId { get; set; }
}