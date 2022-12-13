using Gizo.Application.Models;
using Gizo.Application.Users.Dtos;
using MediatR;

namespace Gizo.Application.Users.Commands;

public sealed record VerifyClientIdentityCommand(
    string Username,
    string VerifyCode) : IRequest<OperationResult<IdentityUserDto>>
{ }
