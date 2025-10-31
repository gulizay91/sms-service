using SmsService.Application.Interfaces.SmsServices.SmsProviders;
using SmsService.Infrastructure.Handlers;
using SmsService.Infrastructure.Services.SmsServices.SmsProviders.IletiMerkezi;
using SmsService.Infrastructure.Settings;

namespace SmsService.API.Registrations;

public static class HttpClientRegister
{
  public static void RegisterHttpClients(this IServiceCollection serviceCollection, IConfiguration configuration)
  {
    serviceCollection.AddTransient<RequestResponseLoggingHandler>();

    serviceCollection.AddIletiMerkeziClient(configuration);
  }

  private static void AddIletiMerkeziClient(this IServiceCollection serviceCollection, IConfiguration configuration)
  {
    var proxySettings = new ProxySettings { Url = string.Empty };
    configuration.GetSection("Services:IletiMerkeziService:ProxySettings").Bind(proxySettings);

    serviceCollection.AddHttpClient<IIletiMerkeziSmsProvider, IletiMerkeziSmsProvider>(
        nameof(IIletiMerkeziSmsProvider),
        client =>
        {
          client.Timeout = TimeSpan.FromSeconds(proxySettings.Timeout);
          client.BaseAddress = new Uri(proxySettings.Url);
        })
      .SetHandlerLifetime(TimeSpan.FromMinutes(5))
      .AddHttpMessageHandler<RequestResponseLoggingHandler>()
      .AddCustomResilienceHandler(proxySettings);
  }
}