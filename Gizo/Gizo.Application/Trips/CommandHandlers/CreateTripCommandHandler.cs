using Gizo.Application.Enums;
using Gizo.Application.Models;
using Gizo.Application.Options;
using Gizo.Application.Trips.Dtos;
using Gizo.Domain.Aggregates.TripAggregate;
using Gizo.Domain.Aggregates.UserAggregate;
using Gizo.Domain.Exceptions;
using Gizo.Domain.Validators.TripValidators;
using Gizo.Infrastructure;
using Gizo.Utility;
using MediatR;
using Microsoft.EntityFrameworkCore;
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

    public async Task<OperationResult<CreatedTripResponse>> Handle(CreateTripCommand request,
        CancellationToken cancellationToken)
    {
        var validator = new TripValidator();
        var chunkSize = FileHelper.MBToByte(_uploadFileSettings.ChunkSize);

        var userCarModel = await GetSelectedCarModel(request.UserId, cancellationToken);
        var trip = Trip.CreateTrip(request.UserId, chunkSize, userCarModel.Id);

        var validationResult = validator.Validate(trip);
        if (!validationResult.IsValid)
        {
            foreach (var error in validationResult.Errors)
            {
                _result.AddError(ErrorCode.ValidationError, error.ErrorMessage);
            }
        }

        await _context.AddAsync(trip, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        _result.Data = new CreatedTripResponse(trip.Id);

        return _result;
    }

    private async Task<UserCarModel> GetSelectedCarModel(long userId, CancellationToken token)
    {
        var user = await _context.Users
            .Include(_ => _.UserCarModels.Where(_ => _.IsSelected))
            .FirstOrDefaultAsync(_ => _.Id == userId, token);

        if (user == null)
        {
            throw new NotFoundException("User");
        }

        if (!user.UserCarModels.Any())
        {
            throw new NotFoundException("User car");
        }

        return user.UserCarModels.First();
    }
}
