using Gizo.Application.ClientIdentity.Dtos;
using Gizo.Application.Models;
using MediatR;

namespace Gizo.Application.ClientIdentity.Commands;

public sealed record VerifyClientIdentityCommand(
    string Username,
    string VerifyCode) : IRequest<OperationResult<ClientIdentityUserDto>>
{ }
