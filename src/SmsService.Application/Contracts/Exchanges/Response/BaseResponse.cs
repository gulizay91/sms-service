using System.Text.Json.Serialization;

namespace SmsService.Application.Contracts.Exchanges.Response;

public class BaseResponse
{
  [JsonConstructor]
  protected BaseResponse(bool success, int statusCode, string? message, List<ErrorDetail>? errors = null)
  {
    Success = success;
    StatusCode = statusCode;
    Message = message;
    Errors = errors;
  }

  public bool Success { get; set; }
  public int StatusCode { get; set; }
  public string? Message { get; set; }

  public List<ErrorDetail>? Errors { get; set; }

  public static BaseResponse SuccessResponse(string? message = "Success")
  {
    return new BaseResponse(true, 200, message);
  }

  public static BaseResponse ErrorResponse(string message, int statusCode = 400, List<ErrorDetail>? errors = null)
  {
    return new BaseResponse(false, statusCode, message, errors);
  }
}

public class ErrorDetail
{
  public string PropertyName { get; set; } = string.Empty;
  public string ErrorMessage { get; set; } = string.Empty;
  public object? AttemptedValue { get; set; }
}