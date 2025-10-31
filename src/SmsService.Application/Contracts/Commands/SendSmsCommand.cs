using SmsService.Application.Contracts.Exchanges.Response;
using SmsService.Application.Interfaces.Wrappers;

namespace SmsService.Application.Contracts.Commands;

public record SendSmsCommand : IRequestWrapper<SendSmsResponse>
{
  public required string PhoneNumber { get; init; }
  public required string Message { get; init; }
}