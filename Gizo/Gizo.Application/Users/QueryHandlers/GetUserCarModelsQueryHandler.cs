using AutoMapper;
using Gizo.Application.Enums;
using Gizo.Application.Models;
using Gizo.Application.Users.Dtos;
using Gizo.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gizo.Application.Users.QueryHandlers;

public sealed record GetUserCarModelsQuery(long UserId)
    : IRequest<OperationResult<List<UserCarResponse>>>;

public class GetUserCarModelsQueryHandler
    : IRequestHandler<GetUserCarModelsQuery, OperationResult<List<UserCarResponse>>>
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public GetUserCarModelsQueryHandler(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<OperationResult<List<UserCarResponse>>> Handle(GetUserCarModelsQuery request,
        CancellationToken cancellationToken)
    {
        var result = new OperationResult<List<UserCarResponse>>();

        var user = await _context.Users
            .Include(_ => _.UserCarModels.Where(x => x.UserId == request.UserId))
            .ThenInclude(_ => _.CarModel.CarBrand)
            .AsNoTracking()
            .FirstOrDefaultAsync(_ => _.Id == request.UserId, cancellationToken);

        if (user == null)
        {
            result.AddError(ErrorCode.NotFound, "Users not found");

            return result;
        }

        result.Data = _mapper.Map<List<UserCarResponse>>(user.UserCarModels);

        return result;
    }
}
