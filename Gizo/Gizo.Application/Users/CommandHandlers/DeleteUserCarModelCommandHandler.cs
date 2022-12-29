using Gizo.Application.Enums;
using Gizo.Application.Models;
using Gizo.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gizo.Application.Users.CommandHandlers;

public sealed record DeleteUserCarModelCommand(long CarModelId,
    long UserId) : IRequest<OperationResult<bool>>;

public class DeleteUserCarModelCommandHandler : IRequestHandler<DeleteUserCarModelCommand, OperationResult<bool>>
{
    private readonly DataContext _context;

    public DeleteUserCarModelCommandHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<OperationResult<bool>> Handle(DeleteUserCarModelCommand request, CancellationToken token)
    {
        var result = new OperationResult<bool>();
        var user = await _context.Users
            .Include(_ => _.UserCarModels)
            .FirstOrDefaultAsync(_ => _.Id == request.UserId, token);

        if (user == null)
        {
            result.AddError(ErrorCode.NotFound, "User not found");

            return result;
        }

        var findCarModel = user.FindUserCarModel(request.CarModelId);

        if(findCarModel == null)
        {
            result.AddError(ErrorCode.NotFound, "CarModel not found");

            return result;
        }

        user.RemoveCar(findCarModel);

        _context.Users.Update(user);
        result.Data = await _context.SaveChangesAsync(token) > 0;

        return result;
    }
}
