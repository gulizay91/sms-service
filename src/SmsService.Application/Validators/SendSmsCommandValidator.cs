using FluentValidation;
using SmsService.Application.Contracts.Commands;
using SmsService.Application.Utils;

namespace SmsService.Application.Validators;

public class SendSmsCommandValidator : AbstractValidator<SendSmsCommand>
{
  public SendSmsCommandValidator()
  {
    RuleFor(x => x.PhoneNumber)
      .Cascade(CascadeMode.Stop)
      .NotEmpty().WithMessage("Phone number is required.")
      .Must(PhoneNumberUtils.IsPhoneNumberFormatValid).WithMessage("Phone number format is invalid.");

    RuleFor(x => x.Message)
      .NotEmpty().WithMessage("Message is required.")
      .MaximumLength(160).WithMessage("Message cannot be longer than 160 characters.");
  }
}