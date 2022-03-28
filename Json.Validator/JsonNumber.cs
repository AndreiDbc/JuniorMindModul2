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
            bool endsWithDot = input.EndsWith(".");
            bool isJsonValid = (IsValidInteger(GetInteger(input, dotIndex)) || IsValidInteger(GetInteger(input, exponentIndex))) &&
                IsValidFraction(GetFraction(input, dotIndex, exponentIndex)) &&
                IsValidExponent(GetExponent(input, exponentIndex));
            return !endsWithDot && isJsonValid;
        }

        private static string GetInteger(string input, int dotIndex, int exponentIndex)
        {
            if (dotIndex == exponentIndex)
            {
                return input;
            }

            return exponentIndex > dotIndex && dotIndex == -1 ? input[0..exponentIndex] : input[0..dotIndex];
        }

        private static string GetFraction(string input, int dotIndex, int exponentIndex)
        {
            if (dotIndex == -1)
            {
                return string.Empty;
            }

            return exponentIndex == -1 ? input[(dotIndex + 1)..] : input[(dotIndex + 1)..exponentIndex];
        }

        private static string GetExponent(string input, int exponentIndex)
        {
            if (exponentIndex == -1)
            {
                return string.Empty;
            }

            return input[(exponentIndex + 1)..];
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

        private static bool NumberHasSign(string input)
        {
            return input.StartsWith("+") || input.StartsWith("-");
        }

        private static bool IsValidInteger(string integer)
        {
            bool isValidInteger = !(integer.StartsWith("0") && integer.Length > 1);
            isValidInteger &= NumberHasSign(integer) ? ContainsOnlyDigits(integer[1..]) : ContainsOnlyDigits(integer);
            return isValidInteger;
        }

        private static bool IsValidExponent(string exponent)
        {
            if (exponent == null)
            {
                return false;
            }

            return ContainsOnlyDigits(exponent) && !exponent.EndsWith("+") && !exponent.EndsWith("-");
        }

        private static bool IsValidFraction(string fraction)
        {
            if (fraction == null)
            {
                return false;
            }

            return ContainsOnlyDigits(fraction);
        }
    }
}
