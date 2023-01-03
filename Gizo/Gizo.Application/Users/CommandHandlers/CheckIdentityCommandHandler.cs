using Gizo.Application.Enums;
using Gizo.Application.Extensions;
using Gizo.Application.Models;
using Gizo.Application.Options;
using Gizo.Domain.Aggregates.UserAggregate;
using Gizo.Domain.Contracts.Services;
using Gizo.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Gizo.Application.Users.CommandHandlers;

public sealed record CheckClientIdentityCommand(string Username) : IRequest<OperationResult<bool>>;

public class CheckIdentityCommandHandler : IRequestHandler<CheckClientIdentityCommand, OperationResult<bool>>
{
    private readonly DataContext _context;
    private readonly IdentityConfigs _settings;
    private readonly ISmsService _smsService;
    private readonly UserManager<User> _userManager;
    private readonly IHostEnvironment _environment;
    private readonly OperationResult<bool> _result = new();

    public CheckIdentityCommandHandler(DataContext context,
        IOptions<IdentityConfigs> identityServerSettings,
        ISmsService smsService,
        UserManager<User> userManager,
        IHostEnvironment environment)
    {
        _context = context;
        _settings = identityServerSettings.Value;
        _smsService = smsService;
        _userManager = userManager;
        _environment = environment;
    }

    public async Task<OperationResult<bool>> Handle(CheckClientIdentityCommand request,
        CancellationToken cancellationToken)
    {
        var (expirationDurationTime, useSampleCode) = _settings;
        try
        {
            var user = await _userManager.FindUserByName(request.Username, _ => _.UserVerificationCodes);
            if (user is null)
                user = await CreateIdentityUserAsync(request);

            if (_result.IsError || user is null)
                return _result;

            var verifyCode = user.GetValidCode(expirationDurationTime, VerificationType.Sms);
            if (verifyCode is null)
            {
                user.CreateCode(useSampleCode, VerificationType.Sms);
                _context.Update(user);
            }

            await  _context.SaveChangesAsync(cancellationToken);

            // if (!_environment.IsDevelopment())
            // await _smsService.Send(user.PhoneNumber, $"Your verification code: {verifyCode.Code}");
            _result.Data = true;
        }
        catch (Exception e)
        {
            _result.AddUnknownError(e.Message);
        }

        return _result;
    }

    private async Task<User?> CreateIdentityUserAsync(CheckClientIdentityCommand request)
    {
        var user = User.CreateUserByPhoneNumber(request.Username);
        var createdIdentity = await _userManager.CreateAsync(user);

        if (createdIdentity.Succeeded)
            return user;

        foreach (var identityError in createdIdentity.Errors)
            _result.AddError(ErrorCode.IdentityCreationFailed, identityError.Description);
        return null;
    }
}
