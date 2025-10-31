using SmsService.API.WorkerServices;
using SmsService.Application.Interfaces.SmsServices;
using SmsService.Application.Interfaces.SmsServices.SmsProviders;
using SmsService.Infrastructure.Services.SmsServices.SmsProviders.Twilio;

namespace SmsService.API.Registrations;

public static class ServiceRegister
{
  public static void RegisterServices(this IServiceCollection serviceCollection)
  {
    serviceCollection.AddScoped<ISmsService, Infrastructure.Services.SmsServices.SmsService>();
    serviceCollection.AddScoped<ITwilioSmsProvider, TwilioSmsProvider>();

    serviceCollection.AddHostedService<ApplicationLifetimeService>();
  }
}