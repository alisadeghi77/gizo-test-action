using Gizo.Application.Enums;
using Gizo.Application.Models;
using Gizo.Application.Services;
using Gizo.Application.Trips.Commands;
using Gizo.Domain.Aggregates.TripAggregate;
using Gizo.Domain.Contracts.Repository;
using MediatR;

namespace Gizo.Application.Trips.CommandHandlers;

public class UploadVideoCompletedCommandHandler
    : IRequestHandler<UploadVideoCompletedCommand, OperationResult<bool>>
{
    private readonly UploadFileService _uploadFileService;
    private readonly IRepository<Trip> _tripRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly OperationResult<bool> _result = new();

    public UploadVideoCompletedCommandHandler(UploadFileService uploadFileService,
        IRepository<Trip> tripRepository,
        IUnitOfWork unitOfWork)
    {
        _uploadFileService = uploadFileService;
        _tripRepository = tripRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<OperationResult<bool>> Handle(
        UploadVideoCompletedCommand request,
        CancellationToken token)
    {
        var trip = await _tripRepository
            .Get()
            .Filter(_ => _.Id == request.Id)
            .Include(_ => _.TripTempVideos)
            .FirstAsync(token);

        if (trip == null)
            _result.AddError(ErrorCode.NotFound, "Trip not found");

        var videoFileName = _uploadFileService.UploadCompleted(
                trip.VideoFilePath,
                request.FileName);

        var updatedModel = Trip.UploadVideoCompleted(
            trip,
            videoFileName);

        trip.RemoveAllTempVideo();

        _tripRepository.Update(updatedModel)
            .OnlyInclude(_ => _.VideoFileName)
            .OnlyInclude(_ => _.ModifyDate)
            .UpdateRelations(_ => _.TripTempVideos);

        _result.Data = await _unitOfWork.SaveChangesAsync(token) > 0;

        return _result;
    }
}
