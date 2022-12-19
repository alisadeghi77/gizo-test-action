using Gizo.Application.Models;
using Gizo.Application.Users.Dtos;
using MediatR;

namespace Gizo.Application.Users.Commands;

public sealed record RegisterIdentityCommand(
    string Username,
    string Password,
    string FirstName,
    string LastName,
    DateTime DateOfBirth,
    string Phone,
    string CurrentCity) : IRequest<OperationResult<CurrentUserDto>>;