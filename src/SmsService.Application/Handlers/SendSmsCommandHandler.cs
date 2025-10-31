using SmsService.Application.Contracts.Commands;
using SmsService.Application.Contracts.Exchanges.Request;
using SmsService.Application.Contracts.Exchanges.Response;
using SmsService.Application.Interfaces.SmsServices;
using SmsService.Application.Interfaces.Wrappers;

namespace SmsService.Application.Handlers;

public class SendSmsCommandHandler : IRequestHandlerWrapper<SendSmsCommand, SendSmsResponse>
{
  private readonly ISmsService _smsService;

  public SendSmsCommandHandler(ISmsService smsService)
  {
    _smsService = smsService;
  }

  public async Task<SendSmsResponse> Handle(SendSmsCommand request, CancellationToken cancellationToken)
  {
    var serviceRequest = new SendSmsRequest(new Receiver(request.PhoneNumber), request.Message);
    var response = await _smsService.SendSmsAsync(serviceRequest);

    return response;
  }
}