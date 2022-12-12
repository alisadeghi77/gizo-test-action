using Gizo.Application.Models;
using MediatR;

namespace Gizo.Application.Identity.Commands;

public sealed record RemoveAccountCommand(
    long IdentityUserId,
    long RequestorGuid) : IRequest<OperationResult<bool>>
{ }