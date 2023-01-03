using Gizo.Application.Enums;
using Gizo.Application.Extensions;
using Gizo.Application.Models;
using Gizo.Application.Options;
using Gizo.Application.Services;
using Gizo.Application.Users.Dtos;
using Gizo.Domain.Aggregates.UserAggregate;
using Gizo.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Gizo.Application.Users.CommandHandlers;

public sealed record VerifyCommand(string Username,
    string VerifyCode) : IRequest<OperationResult<UserVerifyResponse>>;

public class VerifyCommandHandler : IRequestHandler<VerifyCommand, OperationResult<UserVerifyResponse>>
{
    private readonly DbContext _context;
    private readonly UserManager<User> _userManager;
    private readonly IdentityConfigs _settings;
    private readonly IdentityService _identityService;
    private readonly OperationResult<UserVerifyResponse> _result;

    public VerifyCommandHandler(
        DataContext context,
        UserManager<User> userManager,
        IOptions<IdentityConfigs> identityServerSettings,
        IdentityService identityService)
    {
        _result = new();
        _context = context;
        _userManager = userManager;
        _settings = identityServerSettings.Value;
        _identityService = identityService;
    }

    public async Task<OperationResult<UserVerifyResponse>> Handle(VerifyCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var user = await _userManager.FindUserByName(request.Username, _ => _.UserVerificationCodes);
            if (user is null)
            {
                _result.AddError(ErrorCode.IdentityUserDoesNotExist, UserErrorMessages.NonExistentIdentityUser);
                return _result;
            }

            var isCodeValid = user
                .ValidateCode(_settings.CodeExpirationDurationTime, request.VerifyCode, VerificationType.Sms);
            if (!isCodeValid)
            {
                _result.AddError(ErrorCode.IncorrectPassword, UserErrorMessages.InvalidVerificationCode);
                return _result;
            }

            await _context.SaveChangesAsync(cancellationToken);

            _result.Data = new UserVerifyResponse(user.UserName, _identityService.GetJwtString(user));
        }
        catch (Exception e)
        {
            _result.AddUnknownError(e.Message);
        }

        return _result;
    }
}
