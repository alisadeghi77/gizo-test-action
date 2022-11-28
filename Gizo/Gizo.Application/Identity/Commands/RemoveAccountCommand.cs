using Gizo.Application.Models;
using MediatR;

namespace Gizo.Application.Identity.Commands;

public class RemoveAccountCommand : IRequest<OperationResult<bool>>
{
    public long IdentityUserId { get; set; }
    public long RequestorGuid { get; set; }
}