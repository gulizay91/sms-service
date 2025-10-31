using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SmsService.Application.Contracts.Exchanges.Request;
using SmsService.Application.Contracts.Exchanges.Response;
using SmsService.Application.Interfaces.SmsServices.SmsProviders;
using SmsService.Infrastructure.Clients;
using SmsService.Infrastructure.Settings;

namespace SmsService.Infrastructure.Services.SmsServices.SmsProviders.IletiMerkezi;

public class IletiMerkeziSmsProvider : BaseHttpClient, IIletiMerkeziSmsProvider
{
  private readonly IletiMerkeziSettings _iletiMerkeziSettings;
  private readonly ILogger<IletiMerkeziSmsProvider> _logger;

  public IletiMerkeziSmsProvider(HttpClient httpClient, ILogger<IletiMerkeziSmsProvider> logger,
    IOptions<IletiMerkeziSettings> iletiMerkeziSettings) : base(httpClient)
  {
    _logger = logger;
    _iletiMerkeziSettings = iletiMerkeziSettings.Value;
  }

  public bool IsAvailable(string phoneNumberCountryCode)
  {
    return _iletiMerkeziSettings.SmsProviderSettings.Enable &&
           _iletiMerkeziSettings.SmsProviderSettings.ActiveCountryCodes.Contains(phoneNumberCountryCode);
  }

  public async Task<SendSmsResponse> SendSmsAsync(SendSmsRequest sendSmsRequest)
  {
    try
    {
      var requestMessage = new HttpRequestMessage(HttpMethod.Post, "v1/send-sms/json");
      var content = new IletiMerkeziSendSmsRequest(
        new RequestContent(
          new Authentication(
            _iletiMerkeziSettings.Key,
            _iletiMerkeziSettings.Hash
          ),
          new Order(
            _iletiMerkeziSettings.Sender,
            new List<string>(),
            "0",
            "BIREYSEL",
            new Message(
              sendSmsRequest.TextMessage,
              new Receipents(
                new List<string> { sendSmsRequest.Receiver.PhoneNumber }
              )
            )
          )
        )
      );

      var httpResponseMessage =
        await PostAsync<HttpResponseMessage, IletiMerkeziSendSmsRequest>(requestMessage, content);

      if (httpResponseMessage is { IsSuccessStatusCode: false })
      {
        var response = httpResponseMessage.Content.ReadAsStringAsync().Result;
        var resMessage = JsonDocument.Parse(response)
          .RootElement.GetProperty("response")
          .GetProperty("status")
          .GetProperty("message")
          .GetString();
        return resMessage != null
          ? new SendSmsResponse(false, (int)httpResponseMessage.StatusCode, resMessage)
          : new SendSmsResponse(false, (int)httpResponseMessage.StatusCode, "Error sending SMS");
      }

      var message = $"SMS sent: {sendSmsRequest.Receiver.PhoneNumber}";
      _logger.LogInformation(message);
      return new SendSmsResponse(true, (int)httpResponseMessage.StatusCode, message);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Error sending SMS for receiver {0}", sendSmsRequest.Receiver.PhoneNumber);
      return new SendSmsResponse(false, (int)HttpStatusCode.InternalServerError, ex.Message);
    }
  }
}