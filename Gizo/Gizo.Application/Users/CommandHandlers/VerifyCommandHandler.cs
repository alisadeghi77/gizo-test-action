using Gizo.Application.Enums;
using Gizo.Application.Extensions;
using Gizo.Application.Models;
using Gizo.Application.Options;
using Gizo.Application.Services;
using Gizo.Application.Users.Dtos;
using Gizo.Domain.Aggregates.UserAggregate;
using Gizo.Domain.Contracts.Repository;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Gizo.Application.Users.CommandHandlers;

public sealed record VerifyCommand(string Username,
    string VerifyCode) : IRequest<OperationResult<UserVerifyResponse>>;

public class VerifyCommandHandler : IRequestHandler<VerifyCommand, OperationResult<UserVerifyResponse>>
{
    private readonly IUnitOfWork _uow;
    private readonly UserManager<User> _userManager;
    private readonly IdentityConfigs _settings;
    private readonly IdentityService _identityService;
    private OperationResult<UserVerifyResponse> _result = new();

    public VerifyCommandHandler(
        IUnitOfWork uow,
        UserManager<User> userManager,
        IOptions<IdentityConfigs> identityServerSettings,
        IdentityService identityService)
    {
        _uow = uow;
        _userManager = userManager;
        _settings = identityServerSettings.Value;
        _identityService = identityService;
    }

    public async Task<OperationResult<UserVerifyResponse>> Handle(VerifyCommand request,
        CancellationToken token)
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

            await _uow.SaveChangesAsync(token);

            _result.Data = new UserVerifyResponse(user.UserName, _identityService.GetJwtString(user));
        }
        catch (Exception e)
        {
            _result.AddUnknownError(e.Message);
        }

        return _result;
    }
}