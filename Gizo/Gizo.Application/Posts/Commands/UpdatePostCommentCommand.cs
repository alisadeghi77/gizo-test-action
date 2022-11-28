﻿using Gizo.Domain.Aggregates.PostAggregate;
using Gizo.Application.Models;
using MediatR;

namespace Gizo.Application.Posts.Commands;

public class UpdatePostCommentCommand : IRequest<OperationResult<PostComment>>
{
    public long UserProfileId { get; set; }
    public long PostId { get; set; }
    public long CommentId { get; set; }
    public string UpdatedText { get; set; }
}