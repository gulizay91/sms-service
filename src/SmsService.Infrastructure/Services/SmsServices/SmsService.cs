using Microsoft.Extensions.Logging;
using SmsService.Application.Contracts.Exchanges.Request;
using SmsService.Application.Contracts.Exchanges.Response;
using SmsService.Application.Interfaces.SmsServices;
using SmsService.Application.Interfaces.SmsServices.SmsProviders;
using SmsService.Application.Utils;

namespace SmsService.Infrastructure.Services.SmsServices;

public class SmsService : ISmsService
{
  private readonly IIletiMerkeziSmsProvider _iletiMerkeziSmsProvider;
  private readonly ILogger<SmsService> _logger;
  private readonly ITwilioSmsProvider _twilioSmsProvider;

  public SmsService(ITwilioSmsProvider twilioSmsProvider, IIletiMerkeziSmsProvider iletiMerkeziSmsProvider,
    ILogger<SmsService> logger)
  {
    _twilioSmsProvider = twilioSmsProvider;
    _iletiMerkeziSmsProvider = iletiMerkeziSmsProvider;
    _logger = logger;
  }

  public async Task<SendSmsResponse> SendSmsAsync(SendSmsRequest sendSmsRequest)
  {
    var countryCode = PhoneNumberUtils.GetCountryCodeFromPhoneNumber(sendSmsRequest.Receiver.PhoneNumber);
    ArgumentException.ThrowIfNullOrWhiteSpace(countryCode, nameof(sendSmsRequest.Receiver.PhoneNumber));
    if (_iletiMerkeziSmsProvider.IsAvailable(countryCode))
      return await _iletiMerkeziSmsProvider.SendSmsAsync(sendSmsRequest);
    if (_twilioSmsProvider.IsAvailable(countryCode))
      return await _twilioSmsProvider.SendSmsAsync(sendSmsRequest);

    _logger.LogCritical("There is no activated country code for this {ParamName}: {ParamValue}!",
      nameof(sendSmsRequest.Receiver.PhoneNumber),
      PhoneNumberUtils.MaskPhoneNumber(sendSmsRequest.Receiver.PhoneNumber));
    throw new NotImplementedException(
      $"There is no activated country code for this {nameof(sendSmsRequest.Receiver.PhoneNumber)}");
  }
}