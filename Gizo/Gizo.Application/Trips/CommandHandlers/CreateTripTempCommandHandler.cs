using Gizo.Application.Enums;
using Gizo.Application.Models;
using Gizo.Application.Services;
using Gizo.Application.Trips.Commands;
using Gizo.Application.Trips.Dtos;
using Gizo.Domain.Aggregates.TripAggregate;
using Gizo.Domain.Contracts.Repository;
using MediatR;

namespace Gizo.Application.Trips.CommandHandlers;

public class CreateTripTempCommandHandler
    : IRequestHandler<CreateTripTempVideoCommand, OperationResult<TripTempVideoCreatedResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepository<Trip> _tripRepository;
    private readonly UploadFileService _uploadFileService;
    private readonly IRepository<TripTempVideo> _tripTempVideoRepository;
    private readonly OperationResult<TripTempVideoCreatedResponse> _result = new();

    public CreateTripTempCommandHandler(IRepository<Trip> tripRepository,
        UploadFileService uploadFileService,
        IRepository<TripTempVideo> tripTempVideoRepository,
        IUnitOfWork unitOfWork)
    {
        _tripRepository = tripRepository;
        _tripTempVideoRepository = tripTempVideoRepository;
        _uploadFileService = uploadFileService;
        _unitOfWork = unitOfWork;
    }

    public async Task<OperationResult<TripTempVideoCreatedResponse>> Handle(
        CreateTripTempVideoCommand request,
        CancellationToken token)
    {
        var fileName = CreateFileChunkName(request.FileName, request.FileChunkId);

        var trip = await _tripRepository
            .Get()
            .Include(x => x.TripTempVideos.Where(_ => _.TripId == request.TripId))
            .Filter(_ => _.Id == request.TripId)
            .FirstAsync(token);

        if (trip == null)
            _result.AddError(ErrorCode.NotFound, "Trip not found");

        await _uploadFileService.UploadChunk(
           fileName,
           trip.VideoFilePath,
           request.File);

        var tempVideo = trip.AddTempVideo(fileName);

        await _tripTempVideoRepository.InsertAsync(tempVideo, token);
        await _unitOfWork.SaveChangesAsync(token);

        _result.Data = new TripTempVideoCreatedResponse()
        {
            Id = trip.Id,
        };

        return _result;
    }

    private string CreateFileChunkName(string fileName, string fileChunkId)
    {
        return fileName + fileChunkId;
    }
}
