namespace SmsService.Application.Interfaces.SmsServices.SmsProviders;

public interface ITwilioSmsProvider : ISmsService
{
  bool IsAvailable(string phoneNumber);
}