using System;
using PhoneNumbers;

namespace ECommerce.Core.Helpers
{
    public static class GeneralHelpers
    {
        public static bool IsPhoneValid(string number)
        {
            try
            {
                if (!System.Text.RegularExpressions.Regex.IsMatch(number, @"^\+?[0-9]+$"))
                    return false;

                var phoneNumberUtil = PhoneNumberUtil.GetInstance();
                var phoneNumber = phoneNumberUtil.Parse(number, string.Empty);
                var numberType = phoneNumberUtil.GetNumberType(phoneNumber);
                var isValid = phoneNumberUtil.IsValidNumber(phoneNumber);
                if (
                    isValid
                    && (
                        numberType == PhoneNumberType.FIXED_LINE
                        || numberType == PhoneNumberType.MOBILE
                        || numberType == PhoneNumberType.FIXED_LINE_OR_MOBILE
                    )
                )
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
