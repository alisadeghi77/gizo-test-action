using Gizo.Application.Enums;
using Gizo.Application.Extensions;
using Gizo.Application.Models;
using Gizo.Application.Options;
using Gizo.Application.Services;
using Gizo.Application.Users.CommandHandlers;
using Gizo.Domain.Aggregates.UserAggregate;
using Gizo.Domain.Contracts.Repository;
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
    private readonly DataContext _ctx;
    private readonly IdentityConfigs _settings;
    private readonly ISmsService _smsService;
    private readonly IRepository<User> _userRepository;
    private readonly UserManager<User> _userManager;
    private readonly IUnitOfWork _uow;
    private readonly IHostEnvironment _environment;
    private readonly OperationResult<bool> _result = new();

    public CheckIdentityCommandHandler(DataContext ctx,
        IOptions<IdentityConfigs> identityServerSettings,
        ISmsService smsService,
        UserManager<User> userManager,
        IRepository<User> userRepository,
        IUnitOfWork uow,
        IHostEnvironment environment)
    {
        _ctx = ctx;
        _settings = identityServerSettings.Value;
        _smsService = smsService;
        _userManager = userManager;
        _uow = uow;
        _environment = environment;
        _userRepository = userRepository;
    }

    public async Task<OperationResult<bool>> Handle(CheckClientIdentityCommand request, CancellationToken token)
    {
        var (expirationDurationTime, useSampleCode) = _settings;
        try
        {
            var user = await _userManager.FindUserByName(request.Username, _ => _.UserVerificationCodes);
            if (user is null)
                user = await CreateIdentityUserAsync(request, token);

            if (_result.IsError || user is null)
                return _result;

            var verifyCode = user.GetValidCode(expirationDurationTime, VerificationType.Sms);
            if (verifyCode is null)
            {
                verifyCode = user.CreateCode(useSampleCode, VerificationType.Sms);
                _userRepository.Update(user);
            }

            await _uow.SaveChangesAsync(token);

//            if (!_environment.IsDevelopment())
//                await _smsService.Send(user.PhoneNumber, $"Your verification code: {verifyCode.Code}");

            _result.Data = true;
        }
        catch (Exception e)
        {
            _result.AddUnknownError(e.Message);
        }

        return _result;
    }


    private async Task<User?> CreateIdentityUserAsync(CheckClientIdentityCommand request, CancellationToken token)
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