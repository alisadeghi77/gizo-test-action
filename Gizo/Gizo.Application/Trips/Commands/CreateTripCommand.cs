using Gizo.Application.Models;
using Gizo.Application.Trips.Dtos;
using MediatR;

namespace Gizo.Application.Trips.Commands;

public sealed record CreateTripCommand(
    long UserId,
    string WebRootPath) : IRequest<OperationResult<CreatedTripResponse>>
{ }
