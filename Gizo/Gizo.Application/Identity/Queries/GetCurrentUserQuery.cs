using System.Security.Claims;
using Gizo.Application.Identity.Dtos;
using Gizo.Application.Models;
using MediatR;

namespace Gizo.Application.Identity.Queries;

public sealed record GetCurrentUserQuery(
    long UserProfileId,
    ClaimsPrincipal ClaimsPrincipal) : IRequest<OperationResult<IdentityUserProfileDto>>
{ }