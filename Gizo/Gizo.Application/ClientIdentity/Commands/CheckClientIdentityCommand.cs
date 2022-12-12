using Gizo.Application.Models;
using MediatR;

namespace Gizo.Application.ClientIdentity.Commands;

public sealed record CheckClientIdentityCommand(string UserName): IRequest<OperationResult<bool>> { }
