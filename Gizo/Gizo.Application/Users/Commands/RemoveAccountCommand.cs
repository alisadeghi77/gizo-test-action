using Gizo.Application.Models;
using MediatR;

namespace Gizo.Application.Users.Commands;

public sealed record RemoveAccountCommand(
    long IdentityUserId,
    long RequestorGuid) : IRequest<OperationResult<bool>>
{ }