using Gizo.Application.Identity.Dtos;
using Gizo.Application.Models;
using MediatR;

namespace Gizo.Application.Identity.Commands;

public class RegisterIdentity : IRequest<OperationResult<IdentityUserProfileDto>>
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string Phone { get; set; }
    public string CurrentCity { get; set; }
}