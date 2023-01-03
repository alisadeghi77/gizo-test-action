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

    public async Task<OperationResult<bool>> Handle(SelectUserCarModelCommand request,
        CancellationToken cancellationToken)
    {
        var result = new OperationResult<bool>();

        var user = await _context.Users
            .Include(_ => _.UserCarModels)
            .Where(_ => _.Id == request.UserId)
            .FirstOrDefaultAsync(cancellationToken);

        if (user is null)
        {
            result.AddError(ErrorCode.NotFound, "User not found");
            return result;
        }

        user.SelectCar(request.UserCarModelId);

        _context.Users.Update(user);
        result.Data = await _context.SaveChangesAsync(cancellationToken) > 0;

        return result;
    }
}
