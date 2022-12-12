using Gizo.Application.Identity.Dtos;
using Gizo.Application.Models;
using MediatR;

namespace Gizo.Application.Identity.Commands;

public sealed record RegisterIdentityCommand(
    string Username,
    string Password,
    string FirstName,
    string LastName,
    DateTime DateOfBirth,
    string Phone,
    string CurrentCity) : IRequest<OperationResult<IdentityUserProfileDto>>
{ }