using System.Text.RegularExpressions;
using RestaurantReservation.Core.Constants;

namespace RestaurantReservation.Core.Validation
{
    public static class RestaurantValidator
    {
        public static string? ValidateRestaurantName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return ValidationMessages.InputCannotBeEmpty;
            }

            if (name.Length < 2)
            {
                return ValidationMessages.NameTooShort;
            }

            if (name.Length > 100)
            {
                return ValidationMessages.NameTooLong;
            }

            return null;
        }

        public static string? ValidateAddress(string address)
        {
            if (string.IsNullOrWhiteSpace(address))
            {
                return ValidationMessages.InputCannotBeEmpty;
            }

            if (address.Length < 5)
            {
                return ValidationMessages.AddressTooShort;
            }

            if (address.Length > 200)
            {
                return ValidationMessages.AddressTooLong;
            }

            return null;
        }

        public static string? ValidatePhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
            {
                return ValidationMessages.InputCannotBeEmpty;
            }

            if (!Regex.IsMatch(phoneNumber, @"^\+?[1-9][0-9]{7,14}$"))
            {
                return ValidationMessages.PhoneInvalid;
            }

            return null;
        }

        public static string? ValidateTimeSpan(string timeInput)
        {
            if (string.IsNullOrWhiteSpace(timeInput))
            {
                return ValidationMessages.InputCannotBeEmpty;
            }

            if (TimeSpan.TryParse(timeInput, out var timeSpan))
            {
                if (timeSpan < TimeSpan.Zero || timeSpan >= TimeSpan.FromDays(1))
                    return ValidationMessages.TimeOutOfRange;

                return null;
            }

            return ValidationMessages.TimeInvalid;
        }
    }
}