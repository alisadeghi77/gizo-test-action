using Gizo.Application.Enums;
using Gizo.Application.Models;
using Gizo.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gizo.Application.Users.CommandHandlers;

public sealed record DeleteUserCarModelCommand(long UserCarModelId,
    long UserId) : IRequest<OperationResult<bool>>;

public class DeleteUserCarModelCommandHandler : IRequestHandler<DeleteUserCarModelCommand, OperationResult<bool>>
{
    private readonly DataContext _context;

    public DeleteUserCarModelCommandHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<OperationResult<bool>> Handle(DeleteUserCarModelCommand request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<bool>();

        var user = await _context.Users
            .Include(_ => _.UserCarModels)
            .SingleOrDefaultAsync(_ => _.Id == request.UserId, cancellationToken);

        if (user == null)
        {
            result.AddError(ErrorCode.NotFound, "User car model not found");
            return result;
        }

        user.RemoveCar(request.UserCarModelId);
        _context.Users.Update(user);

        result.Data = await _context.SaveChangesAsync(cancellationToken) > 0;
        return result;
    }
}
