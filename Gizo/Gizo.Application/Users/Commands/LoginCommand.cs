using Gizo.Application.Models;
using Gizo.Application.Users.Dtos;
using MediatR;

namespace Gizo.Application.Users.Commands;

public sealed record LoginCommand(
     string Username,
     string Password) : IRequest<OperationResult<CurrentUserDto>>
{ }