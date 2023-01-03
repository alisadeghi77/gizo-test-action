using Gizo.Application.Enums;
using Gizo.Application.Models;
using Gizo.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gizo.Application.Users.CommandHandlers;

public sealed record AddUserCarModelCommand(long CarModelId,
    long UserId,
    string License) : IRequest<OperationResult<bool>>;

public class AddUserCarModelCommandHandler
    : IRequestHandler<AddUserCarModelCommand, OperationResult<bool>>
{
    private readonly DataContext _context;

    public AddUserCarModelCommandHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<OperationResult<bool>> Handle(AddUserCarModelCommand request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<bool>();
        var user = await _context.Users
            .Include(_ => _.UserCarModels)
            .FirstOrDefaultAsync(_ => _.Id == request.UserId, cancellationToken);

        if (user is null)
        {
            result.AddError(ErrorCode.NotFound, "User not found");
            return result;
        }

        var carBrand = await _context.Cars
            .Include(_ => _.CarModels.Where(_ => _.Id == request.CarModelId))
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);

        if (carBrand is null || carBrand.CarModels.Count == 0)
        {
            result.AddError(ErrorCode.NotFound, "Car model not found");
            return result;
        }

        user.AddCar(request.CarModelId, request.License);
        _context.Update(user);
        result.Data = await _context.SaveChangesAsync(cancellationToken) > 0;

        return result;
    }
}
