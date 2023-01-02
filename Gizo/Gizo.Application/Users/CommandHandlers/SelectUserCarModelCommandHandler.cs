using Gizo.Application.Enums;
using Gizo.Application.Models;
using Gizo.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gizo.Application.Users.CommandHandlers;

public sealed record SelectUserCarModelCommand(long UserCarModelId, long UserId) : IRequest<OperationResult<bool>>;

public class SelectUserCarModelCommandHandler : IRequestHandler<SelectUserCarModelCommand, OperationResult<bool>>
{
    private readonly DataContext _context;

    public SelectUserCarModelCommandHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<OperationResult<bool>> Handle(SelectUserCarModelCommand request, CancellationToken token)
    {
        var result = new OperationResult<bool>();

        var user = await _context.Users
            .Include(_ => _.UserCarModels)
            .Where(_ => _.Id == request.UserId)
            .FirstOrDefaultAsync(token);

        if (user is null)
        {
            result.AddError(ErrorCode.NotFound, "User not found");
            return result;
        }

        user.SelectCar(request.UserCarModelId);

        _context.Users.Update(user);
        result.Data = await _context.SaveChangesAsync(token) > 0;

        return result;
    }
}
