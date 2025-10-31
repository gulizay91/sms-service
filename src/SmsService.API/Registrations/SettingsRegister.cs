using SmsService.Infrastructure.Settings;

namespace SmsService.API.Registrations;

public static class SettingsRegister
{
  public static void RegisterSettings(this IServiceCollection serviceCollection, IConfiguration configuration)
  {
    serviceCollection.Configure<TwilioSettings>(
      configuration.GetSection("Services:TwilioService"));

    serviceCollection.Configure<IletiMerkeziSettings>(
      configuration.GetSection("Services:IletiMerkeziService"));

    serviceCollection.ValidateSettings(configuration);
  }

  private static void ValidateSettings(this IServiceCollection serviceCollection, IConfiguration configuration)
  {
    serviceCollection.AddOptions<TwilioSettings>()
      .Bind(configuration.GetSection("Services:TwilioService"))
      .ValidateDataAnnotations()
      .ValidateOnStart();

    serviceCollection.AddOptions<IletiMerkeziSettings>()
      .Bind(configuration.GetSection("Services:IletiMerkeziService"))
      .ValidateDataAnnotations()
      .ValidateOnStart();
  }
}