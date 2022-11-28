﻿using Gizo.Domain.Aggregates.PostAggregate;
using Gizo.Application.Models;
using MediatR;

namespace Gizo.Application.Posts.Queries;

public class GetPostByIdQuery : IRequest<OperationResult<Post>>
{
    public Guid PostId { get; set; }
}