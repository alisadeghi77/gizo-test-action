﻿using Gizo.Domain.Aggregates.PostAggregate;
using Gizo.Application.Models;
using MediatR;

namespace Gizo.Application.Posts.Commands;

public class DeletePostCommand : IRequest<OperationResult<Post>>
{
    public Guid PostId { get; set; }
    public Guid UserProfileId { get; set; }
}