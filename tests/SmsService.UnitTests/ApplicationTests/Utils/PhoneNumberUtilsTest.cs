using SmsService.Application.Utils;

namespace SmsService.UnitTests.ApplicationTests.Utils;

public class PhoneNumberUtilsTest
{
  [Theory]
  [InlineData("+1 (234) 567-8901", "+12345678901")] // Remove spaces, dashes, parentheses
  [InlineData("+90 532 123 4567", "+905321234567")] // Remove spaces
  [InlineData("+44-207-123-4567", "+442071234567")] // Remove dashes
  [InlineData("+61(123)4567890", "+611234567890")] // Remove parentheses
  [InlineData("+34637111222", "+34637111222")] // No changes
  public void NormalizePhoneNumber_WhenContainsSpecialCharacters_ShouldRemoveThem(string input, string expectedOutput)
  {
    var result = PhoneNumberUtils.NormalizePhoneNumber(input);
    Assert.Equal(expectedOutput, result);
  }

  [Theory]
  [InlineData("+123456789", true)] // Valid minimum number
  [InlineData("+34637111222", true)] // Spain number
  [InlineData("+1 234 567 8901", true)] // Valid with spaces
  [InlineData("+61-123-456-789", true)] // Valid with dashes
  [InlineData("+1234", false)] // Too short
  [InlineData("+1234567890123456", false)] // Too long
  [InlineData("1234567890", false)] // No country code
  public void IsPhoneNumberFormatValid_WhenPhoneNumberIsValidOrInvalid_ShouldReturnExpectedResult(string phoneNumber,
    bool expectedResult)
  {
    var result = PhoneNumberUtils.IsPhoneNumberFormatValid(phoneNumber);
    Assert.Equal(expectedResult, result);
  }

  [Theory]
  [InlineData("+34637111222", "+34")]
  [InlineData("+905321234567", "+90")]
  [InlineData("+11234567890", "+1")]
  [InlineData("+99912345678", "")]
  [InlineData("+", "")]
  [InlineData("123456789", "")]
  public void GetCountryCodeFromPhoneNumber_WhenValidOrInvalid_ShouldReturnCorrectCode(string phoneNumber,
    string expectedCountryCode)
  {
    var result = PhoneNumberUtils.GetCountryCodeFromPhoneNumber(phoneNumber);
    Assert.Equal(expectedCountryCode, result);
  }

  [Theory]
  [InlineData("+34637111222", true)]
  [InlineData("+905321234567", true)]
  [InlineData("+11234567890", true)]
  [InlineData("+99912345678", false)]
  [InlineData("+", false)]
  [InlineData("123456789", false)]
  public void IsCountryCodeAllowed_WhenCheckingDifferentNumbers_ShouldReturnExpectedResult(string phoneNumber,
    bool isAllowed)
  {
    var result = !string.IsNullOrEmpty(PhoneNumberUtils.GetCountryCodeFromPhoneNumber(phoneNumber));
    Assert.Equal(isAllowed, result);
  }

  [Theory]
  [InlineData("+905321234567", "+90******4567")] // Turkey
  [InlineData("+12345678901", "+1******8901")] // USA/Canada
  [InlineData("+34637123456", "+34*****3456")] // Spain
  [InlineData("+1 234 567 8901", "+1******8901")] // After Normalize
  public void MaskPhoneNumber_WhenValid_ShouldReturnMaskedNumber(string input, string expectedOutput)
  {
    var result = PhoneNumberUtils.MaskPhoneNumber(input);
    Assert.Equal(expectedOutput, result);
  }

  [Theory]
  [InlineData("+90532")] // Too Short
  [InlineData("+1234")] // Too Short
  public void MaskPhoneNumber_WhenTooShort_ShouldThrowException(string input)
  {
    Assert.Throws<ArgumentException>(() => PhoneNumberUtils.MaskPhoneNumber(input));
  }
}