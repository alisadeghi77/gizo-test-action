using Gizo.Application.Enums;
using Gizo.Application.Models;
using Gizo.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gizo.Application.Users.CommandHandlers;

public sealed record EditUserCarModelCommand(long UserCarModelId,
    long UserId,
    string? Licence) : IRequest<OperationResult<bool>>;

public class EditUserCarModelCommandHandler : IRequestHandler<EditUserCarModelCommand, OperationResult<bool>>
{
    private readonly DataContext _context;

    public EditUserCarModelCommandHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<OperationResult<bool>> Handle(EditUserCarModelCommand request,
        CancellationToken cancellationToken)
    {
        var result = new OperationResult<bool>();

        if (string.IsNullOrWhiteSpace(request.Licence))
        {
            result.AddError(ErrorCode.ValidationError, "Licence is required");
            return result;
        }

        var user = await _context.Users
            .Include(_ => _.UserCarModels
                .Where(x => x.Id == request.UserCarModelId))
            .SingleOrDefaultAsync(_ => _.Id == request.UserId, cancellationToken);

        if (user == null)
        {
            result.AddError(ErrorCode.NotFound, "User car model not found");

            return result;
        }

        user.UpdateUserCarModel(user, request.UserCarModelId, request.Licence);

        _context.Users.Update(user);
        result.Data = await _context.SaveChangesAsync(cancellationToken) > 0;

        return result;
    }
}
