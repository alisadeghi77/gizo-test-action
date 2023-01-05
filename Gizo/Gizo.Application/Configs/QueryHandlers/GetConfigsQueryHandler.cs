using Gizo.Application.Configs.Dtos;
using Gizo.Application.Models;
using Gizo.Application.Options;
using Gizo.Utility;
using MediatR;
using Microsoft.Extensions.Options;

namespace Gizo.Application.Configs.QueryHandlers;

public sealed record GetConfigsQuery : IRequest<OperationResult<GetConfigsResponse>>;

public class GetConfigsQueryHandler : IRequestHandler<GetConfigsQuery, OperationResult<GetConfigsResponse>>
{
    private readonly UploadFileSettings _uploadFileSettings;

    public GetConfigsQueryHandler(IOptions<UploadFileSettings> uploadFileSettings)
    {
        _uploadFileSettings = uploadFileSettings.Value;
    }

    public Task<OperationResult<GetConfigsResponse>> Handle(GetConfigsQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(new OperationResult<GetConfigsResponse>
        {
            Data = new GetConfigsResponse
            {
                AspectRatio = "4:3",
                ChunkSize = FileHelper.MBToByte(_uploadFileSettings.ChunkSize)
            }
        });
    }
}
