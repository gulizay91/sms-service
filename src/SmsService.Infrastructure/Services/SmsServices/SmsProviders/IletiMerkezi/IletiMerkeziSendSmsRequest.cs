using System.Text.Json.Serialization;

namespace SmsService.Infrastructure.Services.SmsServices.SmsProviders.IletiMerkezi;

public record IletiMerkeziSendSmsRequest(
  [property: JsonPropertyName("request")]
  RequestContent Request
);

public record RequestContent(
  Authentication Authentication,
  Order Order
);

public record Authentication(
  string Key,
  string Hash
);

public record Order(
  string Sender,
  List<string> SendDateTime,
  string Iys,
  string IysList,
  Message Message
);

public record Message(
  string Text,
  Receipents Receipents
);

public record Receipents(
  List<string> Number
);