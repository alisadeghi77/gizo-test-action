using Gizo.Application.Models;
using MediatR;

namespace Gizo.Application.Trips.Commands;

public sealed record UploadVideoCompletedCommand(
    long Id,
    string FileName) : IRequest<OperationResult<bool>>;
