using FastEndpoints;
using MediatR;
using SmsService.Application.Contracts.Commands;
using SmsService.Application.Contracts.Exchanges.Response;

namespace SmsService.API.Endpoints;

public class SendSmsEndpointV1 : Endpoint<SendSmsCommand, SendSmsResponse>
{
  private readonly IMediator _mediator;

  public SendSmsEndpointV1(IMediator mediator)
  {
    _mediator = mediator;
  }

  public override void Configure()
  {
    Post("/send-sms");
    Version(1);
    AllowAnonymous();
  }

  public override async Task HandleAsync(SendSmsCommand req, CancellationToken ct)
  {
    var response = await _mediator.Send(req, ct);
    await SendAsync(response, cancellation: ct);
  }
}