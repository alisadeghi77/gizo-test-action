using Gizo.Domain.Aggregates.PostAggregate;
using Gizo.Application.Models;
using MediatR;

namespace Gizo.Application.Posts.Commands;

public class UpdatePostComment : IRequest<OperationResult<PostComment>>
{
    public Guid UserProfileId { get; set; }
    public Guid PostId { get; set; }
    public Guid CommentId { get; set; }
    public string UpdatedText { get; set; }
}