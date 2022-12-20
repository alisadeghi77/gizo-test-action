using Gizo.Application.Enums;
using Gizo.Application.Models;
using Gizo.Application.Services;
using Gizo.Application.Trips.Commands;
using Gizo.Application.Trips.Dtos;
using Gizo.Domain.Aggregates.TripAggregate;
using Gizo.Domain.Contracts.Repository;
using Gizo.Domain.Validators.TripValidators;
using MediatR;

namespace Gizo.Application.Trips.CommandHandlers;

public class CreateTripCommandHandler
    : IRequestHandler<CreateTripCommand, OperationResult<CreatedTripResponse>>
{
    private readonly IRepository<Trip> _tripRepository;
    private readonly UploadFileService _uploadFileService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly OperationResult<CreatedTripResponse> _result = new();

    public CreateTripCommandHandler(IRepository<Trip> tripRepository,
        UploadFileService uploadFileService,
        IUnitOfWork unitOfWork)
    {
        _tripRepository = tripRepository;
        _uploadFileService = uploadFileService;
        _unitOfWork = unitOfWork;
    }

    public async Task<OperationResult<CreatedTripResponse>> Handle(
        CreateTripCommand request,
        CancellationToken token)
    {
        var validator = new TripValidator();

        var createFileResult = _uploadFileService.CreateUserFile(request.WebRootPath);

        var trip = Trip.CreateTrip(
            request.UserId,
            createFileResult.ChunkSize,
            createFileResult.FilePath);

        var validationResult = validator.Validate(trip);
        if (!validationResult.IsValid)
        {
            foreach (var error in validationResult.Errors)
            {
                _result.AddError(ErrorCode.ValidationError, error.ErrorMessage);
            }
        }

        await _tripRepository.InsertAsync(trip, token);
        await _unitOfWork.SaveChangesAsync(token);

        _result.Data = new CreatedTripResponse(trip.Id);

        return _result;
    }
}
