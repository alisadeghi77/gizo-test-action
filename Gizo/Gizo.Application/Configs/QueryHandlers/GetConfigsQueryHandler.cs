using Gizo.Application.Configs.Dtos;
using Gizo.Application.Models;
using MediatR;

namespace Gizo.Application.Configs.QueryHandlers;

public sealed record GetConfigsQuery() : IRequest<OperationResult<GetConfigsResponse>>;

public class GetConfigsQueryHandler : IRequestHandler<GetConfigsQuery, OperationResult<GetConfigsResponse>>
{
    public Task<OperationResult<GetConfigsResponse>> Handle(GetConfigsQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(new OperationResult<GetConfigsResponse>
        {
            Data = new GetConfigsResponse
            {
                AspectRatio = "4:3"
            }
        });
    }
}