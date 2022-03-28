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
                IsValidFraction(GetFraction(input, dotIndex, exponentIndex), dotIndex) &&
                IsValidExponent(GetExponent(input, exponentIndex), exponentIndex);
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

            return exponentIndex == -1 ? input[(dotIndex + 1) ..] : input[(dotIndex + 1) ..exponentIndex];
        }

        private static string GetExponent(string input, int exponentIndex)
        {
            if (exponentIndex == -1)
            {
                return string.Empty;
            }

            return input[(exponentIndex + 1) ..];
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

        private static bool IsValidExponent(string exponent, int exponentIndex)
        {
            if (exponent == null)
            {
                return false;
            }

            bool isValidExponent = NumberHasSign(exponent) ? ContainsOnlyDigits(exponent[1..]) : ContainsOnlyDigits(exponent);
            return isValidExponent || exponentIndex == -1;
        }

        private static bool IsValidFraction(string fraction, int dotIndex)
        {
            if (fraction == null)
            {
                return false;
            }

            return ContainsOnlyDigits(fraction) || dotIndex == -1;
        }
    }
}
