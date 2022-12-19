using System.Security.Claims;
using Gizo.Application.Models;
using Gizo.Application.Users.Dtos;
using MediatR;

namespace Gizo.Application.Users.Queries;

public sealed record GetCurrentUserQuery(
    long userId,
    ClaimsPrincipal ClaimsPrincipal) : IRequest<OperationResult<UserProfileDto>>
{ }