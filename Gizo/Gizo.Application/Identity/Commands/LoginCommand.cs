using Gizo.Application.Identity.Dtos;
using Gizo.Application.Models;
using MediatR;

namespace Gizo.Application.Identity.Commands;

public sealed record LoginCommand(
     string Username,
     string Password) : IRequest<OperationResult<IdentityUserProfileDto>>
{ }