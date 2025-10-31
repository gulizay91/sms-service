using System.Text.Json.Serialization;

namespace SmsService.Application.Contracts.Exchanges.Response;

public class ServiceResponse<T> : BaseResponse
{
  [JsonConstructor]
  public ServiceResponse(
    bool success,
    int statusCode,
    string? message,
    string? errorCode,
    T? data,
    List<ErrorDetail>? errors = null
  ) : base(success, statusCode, message, errors)
  {
    ErrorCode = errorCode;
    Data = data;
  }

  [JsonPropertyName("errorCode")] public string? ErrorCode { get; set; }

  [JsonPropertyName("data")] public T? Data { get; set; }
}