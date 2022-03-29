using System;

namespace Json
{
    public static class JsonNumber
    {
        public static bool IsJsonNumber(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return false;
            }

            int dotIndex = input.IndexOf('.');
            int exponentIndex = input.IndexOfAny(new[] { 'e', 'E' });
            return IsValidInteger(GetInteger(input, dotIndex, exponentIndex)) &&
                IsValidFraction(GetFraction(input, dotIndex, exponentIndex)) &&
                IsValidExponent(GetExponent(input, exponentIndex));
        }

        private static string GetInteger(string input, int dotIndex, int exponentIndex)
        {
            if (dotIndex != -1)
            {
                return input[..dotIndex];
            }

            return exponentIndex != -1 ? input[..exponentIndex] : input;
        }

        private static string GetFraction(string input, int dotIndex, int exponentIndex)
        {
            if (dotIndex == -1)
            {
                return string.Empty;
            }

            return exponentIndex == -1 ? input[dotIndex ..] : input[dotIndex ..exponentIndex];
        }

        private static string GetExponent(string input, int exponentIndex)
        {
            return exponentIndex != -1 ? input[exponentIndex..] : string.Empty;
        }

        private static bool ContainsOnlyDigits(string input)
        {
            foreach (char character in input)
            {
                if (!char.IsDigit(character))
                {
                    return false;
                }
            }

            return input.Length > 0;
        }

        private static bool HasSign(string input)
        {
            return input.StartsWith("+") || input.StartsWith("-");
        }

        private static bool IsValidInteger(string integer)
        {
            bool isValidInteger = !integer.StartsWith("0") || integer.Length <= 1;
            isValidInteger &= ContainsOnlyDigits(integer.StartsWith('-') ? integer[1..] : integer);
            return isValidInteger;
        }

        private static bool IsValidExponent(string exponent)
        {
            if (exponent == string.Empty)
            {
                return true;
            }

            exponent = exponent[1..];
            exponent = HasSign(exponent) ? exponent[1..] : exponent;
            return ContainsOnlyDigits(exponent);
        }

        private static bool IsValidFraction(string fraction)
        {
            if (fraction == string.Empty)
            {
                return true;
            }

            return ContainsOnlyDigits(fraction[1..]) && !(fraction.StartsWith(".") && fraction.Length == 1);
        }
    }
}
