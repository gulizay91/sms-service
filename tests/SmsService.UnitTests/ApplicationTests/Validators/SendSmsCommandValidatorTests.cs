using FluentValidation.TestHelper;
using SmsService.Application.Contracts.Commands;
using SmsService.Application.Validators;

namespace SmsService.UnitTests.ApplicationTests.Validators;

public class SendSmsCommandValidatorTests
{
  private readonly SendSmsCommandValidator _validator = new();

  [Fact]
  public void Validate_ValidCommand_NoErrors()
  {
    var cmd = new SendSmsCommand
    {
      PhoneNumber = "+1234567890",
      Message = "Hello"
    };

    var result = _validator.TestValidate(cmd);
    result.ShouldNotHaveAnyValidationErrors();
  }

  [Fact]
  public void Validate_EmptyPhone_ReturnsError()
  {
    var cmd = new SendSmsCommand
    {
      PhoneNumber = string.Empty,
      Message = "Hello"
    };

    var result = _validator.TestValidate(cmd);
    result.ShouldHaveValidationErrorFor(x => x.PhoneNumber);
  }

  [Fact]
  public void Validate_InvalidPhoneFormat_ReturnsError()
  {
    var cmd = new SendSmsCommand
    {
      PhoneNumber = "invalid-phone",
      Message = "Hello"
    };

    var result = _validator.TestValidate(cmd);
    result.ShouldHaveValidationErrorFor(x => x.PhoneNumber);
  }

  [Fact]
  public void Validate_EmptyMessage_ReturnsError()
  {
    var cmd = new SendSmsCommand
    {
      PhoneNumber = "+1234567890",
      Message = string.Empty
    };

    var result = _validator.TestValidate(cmd);
    result.ShouldHaveValidationErrorFor(x => x.Message);
  }

  [Fact]
  public void Validate_MessageTooLong_ReturnsError()
  {
    var longMessage = new string('a', 161);
    var cmd = new SendSmsCommand
    {
      PhoneNumber = "+1234567890",
      Message = longMessage
    };

    var result = _validator.TestValidate(cmd);
    result.ShouldHaveValidationErrorFor(x => x.Message);
  }
}