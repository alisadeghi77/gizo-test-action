﻿using Gizo.Domain.Aggregates.PostAggregate;
using Gizo.Application.Models;
using MediatR;

namespace Gizo.Application.Posts.Commands;

public class CreatePost : IRequest<OperationResult<Post>>
{
    public Guid UserProfileId { get; set; }
    public string TextContent { get; set; }
}