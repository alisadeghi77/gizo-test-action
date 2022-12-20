using Gizo.Application.Models;
using Gizo.Application.Trips.Dtos;
using MediatR;

namespace Gizo.Application.Trips.Commands;

public sealed record CreateTripTempVideoCommand(
    long TripId,
    string FileChunkId,
    string FileName,
    Stream File) : IRequest<OperationResult<TripTempVideoCreatedResponse>>
{ }
