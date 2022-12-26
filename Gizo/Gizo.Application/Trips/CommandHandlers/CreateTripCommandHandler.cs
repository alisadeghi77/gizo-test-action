using Gizo.Application.Enums;
using Gizo.Application.Models;
using Gizo.Application.Options;
using Gizo.Application.Services;
using Gizo.Application.Trips.Dtos;
using Gizo.Domain.Aggregates.TripAggregate;
using Gizo.Domain.Contracts.Repository;
using Gizo.Domain.Validators.TripValidators;
using Gizo.Infrastructure;
using Gizo.Utility;
using MediatR;
using Microsoft.Extensions.Options;

namespace Gizo.Application.Trips.CommandHandlers;

public sealed record CreateTripCommand(long UserId) : IRequest<OperationResult<CreatedTripResponse>>;

public class CreateTripCommandHandler
    : IRequestHandler<CreateTripCommand, OperationResult<CreatedTripResponse>>
{
    private readonly DataContext _context;
    private readonly OperationResult<CreatedTripResponse> _result = new();
    private readonly UploadFileSettings _uploadFileSettings;

    public CreateTripCommandHandler(DataContext context,
        IOptions<UploadFileSettings> uploadFileSettings)
    {
        _context = context;
        _uploadFileSettings = uploadFileSettings.Value;
    }

    public async Task<OperationResult<CreatedTripResponse>> Handle(
        CreateTripCommand request,
        CancellationToken token)
    {
        var validator = new TripValidator();
        var chunkSize = FileHelper.MBToByte(_uploadFileSettings.ChunkSize);

        var trip = Trip.CreateTrip(request.UserId, chunkSize);

        var validationResult = validator.Validate(trip);
        if (!validationResult.IsValid)
        {
            foreach (var error in validationResult.Errors)
            {
                _result.AddError(ErrorCode.ValidationError, error.ErrorMessage);
            }
        }

        await _context.AddAsync(trip, token);
        await _context.SaveChangesAsync(token);

        _result.Data = new CreatedTripResponse(trip.Id, chunkSize);

        return _result;
    }
}
