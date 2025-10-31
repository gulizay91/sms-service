using System.Text.RegularExpressions;

namespace SmsService.Application.Utils;

public static class PhoneNumberUtils
{
  // Regex pattern to validate phone number format
  //private const string PhoneNumberPattern = @"^\+(\d{1,4})\d{11}$";
  private const string PhoneNumberPattern = @"^\+\d{8,14}$";

  // Dictionary of country codes and their corresponding country abbreviations
  private static readonly Dictionary<string, string> CountryCodes = new()
  {
    { "+1", "US/CA" }, // United States / Canada
    { "+7", "RU/KZ" }, // Russia / Kazakhstan
    { "+20", "EG" }, // Egypt
    { "+27", "ZA" }, // South Africa
    { "+30", "GR" }, // Greece
    { "+31", "NL" }, // Netherlands
    { "+32", "BE" }, // Belgium
    { "+33", "FR" }, // France
    { "+34", "ES" }, // Spain
    { "+36", "HU" }, // Hungary
    { "+39", "IT" }, // Italy
    { "+40", "RO" }, // Romania
    { "+41", "CH" }, // Switzerland
    { "+42", "CZ" }, // Czech Republic
    { "+43", "AT" }, // Austria
    { "+44", "GB" }, // United Kingdom
    { "+45", "DK" }, // Denmark
    { "+46", "SE" }, // Sweden
    { "+47", "NO" }, // Norway
    { "+48", "PL" }, // Poland
    { "+49", "DE" }, // Germany
    { "+51", "PE" }, // Peru
    { "+52", "MX" }, // Mexico
    { "+53", "CU" }, // Cuba
    { "+54", "AR" }, // Argentina
    { "+55", "BR" }, // Brazil
    { "+56", "CL" }, // Chile
    { "+57", "CO" }, // Colombia
    { "+58", "VE" }, // Venezuela
    { "+60", "MY" }, // Malaysia
    { "+61", "AU" }, // Australia
    { "+62", "ID" }, // Indonesia
    { "+63", "PH" }, // Philippines
    { "+64", "NZ" }, // New Zealand
    { "+65", "SG" }, // Singapore
    { "+66", "TH" }, // Thailand
    { "+81", "JP" }, // Japan
    { "+82", "KR" }, // South Korea
    { "+84", "VN" }, // Vietnam
    { "+86", "CN" }, // China
    { "+90", "TR" }, // Turkey
    { "+91", "IN" }, // India
    { "+92", "PK" }, // Pakistan
    { "+93", "AF" }, // Afghanistan
    { "+94", "LK" }, // Sri Lanka
    { "+95", "MM" }, // Myanmar
    { "+98", "IR" }, // Iran
    { "+211", "SS" }, // South Sudan
    { "+212", "MA" }, // Morocco
    { "+213", "DZ" }, // Algeria
    { "+216", "TN" }, // Tunisia
    { "+218", "LY" }, // Libya
    { "+220", "GM" }, // Gambia
    { "+221", "SN" }, // Senegal
    { "+222", "MR" }, // Mauritania
    { "+223", "ML" }, // Mali
    { "+224", "GN" }, // Guinea
    { "+225", "CI" }, // Ivory Coast
    { "+226", "BF" }, // Burkina Faso
    { "+227", "NE" }, // Niger
    { "+228", "TG" }, // Togo
    { "+229", "BJ" }, // Benin
    { "+230", "MU" }, // Mauritius
    { "+231", "LR" }, // Liberia
    { "+232", "SL" }, // Sierra Leone
    { "+233", "GH" }, // Ghana
    { "+234", "NG" }, // Nigeria
    { "+235", "TD" }, // Chad
    { "+236", "CF" }, // Central African Republic
    { "+237", "CM" }, // Cameroon
    { "+238", "CV" }, // Cape Verde
    { "+239", "ST" }, // Sao Tome and Principe
    { "+240", "GQ" }, // Equatorial Guinea
    { "+241", "GA" }, // Gabon
    { "+242", "CG" }, // Republic of Congo
    { "+243", "CD" }, // Democratic Republic of Congo
    { "+244", "AO" }, // Angola
    { "+245", "GW" }, // Guinea-Bissau
    { "+246", "IO" }, // British Indian Ocean Territory
    { "+247", "AC" }, // Ascension Island
    { "+248", "SC" }, // Seychelles
    { "+249", "SD" }, // Sudan
    { "+250", "RW" }, // Rwanda
    { "+251", "ET" }, // Ethiopia
    { "+252", "SO" }, // Somalia
    { "+253", "DJ" }, // Djibouti
    { "+254", "KE" }, // Kenya
    { "+255", "TZ" }, // Tanzania
    { "+256", "UG" }, // Uganda
    { "+257", "BI" }, // Burundi
    { "+258", "MZ" }, // Mozambique
    { "+260", "ZM" }, // Zambia
    { "+261", "MG" }, // Madagascar
    { "+262", "RE" }, // Réunion
    { "+263", "ZW" }, // Zimbabwe
    { "+264", "NA" }, // Namibia
    { "+265", "MW" }, // Malawi
    { "+266", "LS" }, // Lesotho
    { "+267", "BW" }, // Botswana
    { "+268", "SZ" }, // Eswatini
    { "+269", "KM" }, // Comoros
    { "+290", "SH" }, // Saint Helena
    { "+291", "ER" }, // Eritrea
    { "+297", "AW" }, // Aruba
    { "+298", "FO" }, // Faroe Islands
    { "+299", "GL" }, // Greenland
    { "+350", "GI" }, // Gibraltar
    { "+351", "PT" }, // Portugal
    { "+352", "LU" }, // Luxembourg
    { "+353", "IE" }, // Ireland
    { "+354", "IS" }, // Iceland
    { "+355", "AL" }, // Albania
    { "+356", "MT" }, // Malta
    { "+357", "CY" }, // Cyprus
    { "+358", "FI" }, // Finland
    { "+359", "BG" }, // Bulgaria
    { "+370", "LT" }, // Lithuania
    { "+371", "LV" }, // Latvia
    { "+372", "EE" }, // Estonia
    { "+373", "MD" }, // Moldova
    { "+374", "AM" }, // Armenia
    { "+375", "BY" }, // Belarus
    { "+376", "AD" }, // Andorra
    { "+377", "MC" }, // Monaco
    { "+378", "SM" }, // San Marino
    { "+379", "VA" }, // Vatican City
    { "+380", "UA" }, // Ukraine
    { "+381", "RS" }, // Serbia
    { "+382", "ME" }, // Montenegro
    { "+383", "XK" }, // Kosovo
    { "+385", "HR" }, // Croatia
    { "+386", "SI" }, // Slovenia
    { "+387", "BA" }, // Bosnia and Herzegovina
    { "+388", "YU" }, // Former Yugoslavia (this code is no longer in use)
    { "+389", "MK" } // North Macedonia
  };

  // Method to check if the phone number starts with a given country code
  private static bool IsPhoneNumberAllowed(string phoneNumber, string countryCode)
  {
    var normalizedNumber = NormalizePhoneNumber(phoneNumber);
    return normalizedNumber.StartsWith(countryCode);
  }

  // Method to get the country code from the phone number
  public static string GetCountryCodeFromPhoneNumber(string phoneNumber)
  {
    if (!IsPhoneNumberFormatValid(phoneNumber))
      return string.Empty;
    foreach (var code in CountryCodes.Keys)
      if (IsPhoneNumberAllowed(phoneNumber, code))
        return code;
    return string.Empty; // no country code matches
  }


  // Method to validate phone number format using regex
  public static bool IsPhoneNumberFormatValid(string phoneNumber)
  {
    var normalizedNumber = NormalizePhoneNumber(phoneNumber);

    // Create regex instance with the pattern
    var regex = new Regex(PhoneNumberPattern);

    // Check if the phone number matches the pattern
    return regex.IsMatch(normalizedNumber);
  }

  public static string NormalizePhoneNumber(string phoneNumber)
  {
    if (string.IsNullOrWhiteSpace(phoneNumber))
      throw new ArgumentException("Phone number cannot be null or empty.");

    // Remove spaces, dashes, parentheses, and other unwanted characters
    return Regex.Replace(phoneNumber, @"[\s\-\(\)]", "");
  }

  public static string MaskPhoneNumber(string phoneNumber, int visibleDigits = 4)
  {
    if (!IsPhoneNumberFormatValid(phoneNumber))
      throw new ArgumentException("Invalid phone number format.");

    var normalizedNumber = NormalizePhoneNumber(phoneNumber);
    var countryCode = GetCountryCodeFromPhoneNumber(normalizedNumber);

    if (string.IsNullOrEmpty(countryCode))
      throw new ArgumentException("Invalid country code in the phone number.");

    var localNumber = normalizedNumber.Substring(countryCode.Length);

    if (localNumber.Length <= visibleDigits)
      throw new ArgumentException("Local number is too short to leave visible digits.");

    var maskLength = localNumber.Length - visibleDigits;
    var maskedLocalNumber = new string('*', maskLength) + localNumber.Substring(maskLength);

    return countryCode + maskedLocalNumber;
  }
}