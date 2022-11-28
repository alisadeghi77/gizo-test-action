using Gizo.Domain.Aggregates.PostAggregate;
using Gizo.Application.Models;
using MediatR;

namespace Gizo.Application.Posts.Commands;

public class CreatePostCommand : IRequest<OperationResult<Post>>
{
    public long UserProfileId { get; set; }
    public string TextContent { get; set; }
}