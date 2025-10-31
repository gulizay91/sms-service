using System.ComponentModel.DataAnnotations;

namespace SmsService.Infrastructure.Settings;

public record ProxySettings
{
  [Required(ErrorMessage = "Url is required")]
  public required string Url { get; init; }

  [Required(ErrorMessage = "Timeout is required")]
  public int Timeout { get; init; } = 3;

  [Required(ErrorMessage = "RetryAttempts is required")]
  public int RetryAttempts { get; init; } = 5;
}