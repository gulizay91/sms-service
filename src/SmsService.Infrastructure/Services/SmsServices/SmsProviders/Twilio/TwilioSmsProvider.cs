using System.Net;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SmsService.Application.Contracts.Exchanges.Request;
using SmsService.Application.Contracts.Exchanges.Response;
using SmsService.Application.Interfaces.SmsServices.SmsProviders;
using SmsService.Infrastructure.Settings;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace SmsService.Infrastructure.Services.SmsServices.SmsProviders.Twilio;

public class TwilioSmsProvider : ITwilioSmsProvider
{
  private readonly ILogger<TwilioSmsProvider> _logger;
  private readonly TwilioSettings _twilioSettings;

  public TwilioSmsProvider(ILogger<TwilioSmsProvider> logger, IOptions<TwilioSettings> twilioSettings)
  {
    _logger = logger;
    _twilioSettings = twilioSettings.Value;
  }

  public bool IsAvailable(string phoneNumberCountryCode)
  {
    return _twilioSettings.SmsProviderSettings.Enable &&
           _twilioSettings.SmsProviderSettings.ActiveCountryCodes.Contains(phoneNumberCountryCode);
  }

  public async Task<SendSmsResponse> SendSmsAsync(SendSmsRequest sendSmsRequest)
  {
    try
    {
      var messageResource = await MessageResource.CreateAsync(
        new PhoneNumber(sendSmsRequest.Receiver.PhoneNumber),
        from: new PhoneNumber(_twilioSettings.PhoneNumber),
        body: sendSmsRequest.TextMessage
      );

      var message = $"SMS sent: {messageResource.Sid}";
      _logger.LogInformation(message);
      return new SendSmsResponse(true, (int)HttpStatusCode.OK, message);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Error sending SMS for receiver {0}", sendSmsRequest.Receiver.PhoneNumber);
      return new SendSmsResponse(false, (int)HttpStatusCode.InternalServerError, ex.Message);
    }
  }
}