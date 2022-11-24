using System.Security.Claims;
using Gizo.Application.Identity.Dtos;
using Gizo.Application.Models;
using MediatR;

namespace Gizo.Application.Identity.Queries;

public class GetCurrentUser : IRequest<OperationResult<IdentityUserProfileDto>>
{
    public Guid UserProfileId { get; set; }
    public ClaimsPrincipal ClaimsPrincipal { get; set; }
}