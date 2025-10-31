using System.ComponentModel.DataAnnotations;

namespace SmsService.Infrastructure.Settings;

public record TwilioSettings
{
  [Required(ErrorMessage = "Account SID is required")]
  public required string AccountSID { get; init; }

  [Required(ErrorMessage = "Auth Token is required")]
  public required string AuthToken { get; init; }

  [Required(ErrorMessage = "Phone Number is required")]
  public required string PhoneNumber { get; init; }

  public required SmsProviderSettings SmsProviderSettings { get; init; }
}