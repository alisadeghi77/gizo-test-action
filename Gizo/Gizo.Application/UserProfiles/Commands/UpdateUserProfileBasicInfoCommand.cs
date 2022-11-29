﻿using AutoMapper;
using Gizo.Application.Models;
using Gizo.Domain.Aggregates.UserProfileAggregate;
using MediatR;

namespace Gizo.Application.UserProfiles.Commands;

public class UpdateUserProfileBasicInfoCommand : IRequest<OperationResult<UserProfile>>
{
    public long UserProfileId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string EmailAddress { get; set; }
    public string Phone { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string CurrentCity { get; set; }
}
