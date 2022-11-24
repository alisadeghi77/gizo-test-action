using Gizo.Application.Identity.Dtos;
using Gizo.Application.Models;
using MediatR;

namespace Gizo.Application.Identity.Commands;

public class LoginCommand : IRequest<OperationResult<IdentityUserProfileDto>>
{
    public string Username { get; set; }
    public string Password { get; set; }
}