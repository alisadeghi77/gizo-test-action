using Gizo.Application.ClientIdentity.Dtos;
using Gizo.Application.Models;
using MediatR;

namespace Gizo.Application.ClientIdentity.Commands;

public class VerifyClientIdentityCommand : IRequest<OperationResult<ClientIdentityUserDto>>
{
    public string Username { get; set; }

    public string VerifyCode { get; set; }
}
