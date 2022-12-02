using Gizo.Application.Models;
using MediatR;

namespace Gizo.Application.ClientIdentity.Commands;

public class CheckClientIdentityCommand : IRequest<OperationResult<bool>>
{
    public string Username { get; set; }
}
