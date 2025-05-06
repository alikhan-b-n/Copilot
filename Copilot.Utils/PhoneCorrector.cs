using System.Text.RegularExpressions;

namespace Copilot.Utils;

public static class PhoneCorrector
{
    /// <summary>
    /// Convert phone number to digit format.
    /// Also Kazahstani and Russian numbers started with 8, corrects to 7.
    /// Ex: "+7 (707) 333-53-77" -> "77073035370".
    /// </summary>
    public static string ConvertPhoneNumberToDigits(string phoneNumber)
    {
        if (string.IsNullOrEmpty(phoneNumber)) return phoneNumber;

        var correctPhone = Regex.Replace(phoneNumber, @"\D", string.Empty);

        if ((correctPhone.StartsWith("87") || correctPhone.StartsWith("89"))
            && correctPhone.Length == 11)
            correctPhone = "7" + correctPhone[1..];

        return correctPhone;
    }
}