namespace SmsService.Application.Contracts.Exchanges.Response;

public class SendSmsResponse(bool success, int statusCode, string? message)
  : BaseResponse(success, statusCode, message);