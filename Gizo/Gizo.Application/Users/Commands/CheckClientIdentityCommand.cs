using Gizo.Application.Models;
using MediatR;

namespace Gizo.Application.Users.Commands;

public sealed record CheckClientIdentityCommand(string Username): IRequest<OperationResult<bool>> { }
