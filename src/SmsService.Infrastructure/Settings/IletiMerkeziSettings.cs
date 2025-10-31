using System.ComponentModel.DataAnnotations;

namespace SmsService.Infrastructure.Settings;

public record IletiMerkeziSettings
{
  [Required(ErrorMessage = "Api Key is required")]
  public required string Key { get; init; }

  [Required(ErrorMessage = "Api Hash is required")]
  public required string Hash { get; init; }

  [Required(ErrorMessage = "Sender is required")]
  public required string Sender { get; init; }

  public required ProxySettings ProxySettings { get; init; }

  public required SmsProviderSettings SmsProviderSettings { get; init; }
}