﻿using Gizo.Domain.Aggregates.UserProfileAggregate;
using Gizo.Application.Models;
using MediatR;

namespace Gizo.Application.UserProfiles.Queries;

public class GetUserProfileById : IRequest<OperationResult<UserProfile>>
{
    public Guid UserProfileId { get; set; }
}
