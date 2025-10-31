using SmsService.Application.Contracts.Exchanges.Request;
using SmsService.Application.Contracts.Exchanges.Response;

namespace SmsService.Application.Interfaces.SmsServices;

public interface ISmsService
{
  Task<SendSmsResponse> SendSmsAsync(SendSmsRequest sendSmsRequest);
}