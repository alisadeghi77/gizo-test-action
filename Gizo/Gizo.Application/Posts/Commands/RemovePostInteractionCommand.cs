﻿using Gizo.Domain.Aggregates.PostAggregate;
using Gizo.Application.Models;
using MediatR;

namespace Gizo.Application.Posts.Commands;

public sealed record RemovePostInteractionCommand(
    long PostId,
    long InteractionId,
    long UserProfileId
    ) : IRequest<OperationResult<PostInteraction>>
{ }
