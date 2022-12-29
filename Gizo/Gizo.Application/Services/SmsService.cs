using Gizo.Application.Options;
using Gizo.Domain.Contracts.Services;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace Gizo.Application.Services;

public class SmsService : ISmsService
{
    private readonly SmsConfigs _smsConfigs;

    public SmsService(SmsConfigs smsConfigs)
    {
        _smsConfigs = smsConfigs;
    }

    public async Task Send(string phoneNumber, string body)
    {
        TwilioClient.Init(_smsConfigs.AccountSid, _smsConfigs.AuthToken);
        await MessageResource.CreateAsync(
            body: body,
            from: new PhoneNumber(_smsConfigs.SenderPhoneNumber),
            to: new PhoneNumber(phoneNumber)
        );
    }
}