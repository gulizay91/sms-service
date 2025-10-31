namespace SmsService.Application.Interfaces.SmsServices.SmsProviders;

public interface IIletiMerkeziSmsProvider : ISmsService
{
  bool IsAvailable(string phoneNumber);
}