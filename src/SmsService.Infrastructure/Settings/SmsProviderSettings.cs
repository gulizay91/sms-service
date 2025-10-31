using System.ComponentModel.DataAnnotations;

namespace SmsService.Infrastructure.Settings;

public record SmsProviderSettings
{
  [Required(ErrorMessage = "Enable is required")]
  public bool Enable { get; init; }

  [Required(ErrorMessage = "ActiveCountryCodes is required")]
  public required string[] ActiveCountryCodes { get; init; }
}