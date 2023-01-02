using AutoMapper;
using Gizo.Application.Enums;
using Gizo.Application.Models;
using Gizo.Application.Users.Dtos;
using Gizo.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gizo.Application.Users.QueryHandlers;

public sealed record GetUserCarModelDetailsQuery(long Id, long UserId) : IRequest<OperationResult<UserCarResponse>>;

public class GetUserCarModelDetailsQueryHandler : IRequestHandler<GetUserCarModelDetailsQuery, OperationResult<UserCarResponse>>
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public GetUserCarModelDetailsQueryHandler(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<OperationResult<UserCarResponse>> Handle(GetUserCarModelDetailsQuery request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<UserCarResponse>();

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

        var userCarModel = user.UserCarModels.FirstOrDefault(t => t.Id == request.Id);
        if (userCarModel == null)
        {
            result.AddError(ErrorCode.NotFound, "User car model not found");
            return result;
        }

        result.Data = _mapper.Map<UserCarResponse>(userCarModel);

        return result;
    }
}
