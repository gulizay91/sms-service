namespace SmsService.Application.Contracts.Exchanges.Request;

public record SendSmsRequest
{
  public SendSmsRequest(Receiver receiver, string textMessage)
  {
    if (string.IsNullOrWhiteSpace(receiver.PhoneNumber))
      throw new ArgumentException("Phone number cannot be empty", nameof(receiver.PhoneNumber));
    if (string.IsNullOrWhiteSpace(textMessage))
      throw new ArgumentException("Message cannot be empty", nameof(textMessage));
    Receiver = receiver;
    TextMessage = textMessage;
  }

  public Receiver Receiver { get; init; }
  public string TextMessage { get; init; }
}

public record Receiver
{
  public Receiver(string phoneNumber)
  {
    if (string.IsNullOrWhiteSpace(phoneNumber))
      throw new ArgumentException("Phone number cannot be empty", nameof(phoneNumber));
    PhoneNumber = phoneNumber;
  }

  public string PhoneNumber { get; init; }
}