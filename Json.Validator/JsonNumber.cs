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
            if (dotIndex != -1)
            {
                if (exponentIndex != -1 && dotIndex < exponentIndex)
                {
                    return input.Substring(dotIndex + 1, input.Length - 1 - exponentIndex);
                }

                return input.Substring(dotIndex + 1);
            }

            return string.Empty;
        }

        private static string GetExponent(string input, int exponentIndex)
        {
            if (exponentIndex != -1)
            {
                string exponent = input.Substring(exponentIndex + 1);

                if (exponent == string.Empty)
                {
                    return null;
                }

                return exponent;
            }

            return string.Empty;
        }

        private static bool ContainsDigits(string input)
        {
            for (int i = 0; i < input.Length; i++)
            {
                if (!char.IsDigit(input[i]))
                {
                    return false;
                }
            }

            return input.Length > 0;
        }

        private static bool IsValidInteger(string integer)
        {
            return !(integer.StartsWith("0") && !integer.Contains(".") && integer.Length > 1) && ContainsDigits(integer);
        }

        private static bool IsValidExponent(string exponent)
        {
            if (exponent == null)
            {
                return false;
            }

            return ContainsDigits(exponent) && !exponent.EndsWith("+") && !exponent.EndsWith("-");
        }

        private static bool IsValidFraction(string fraction)
        {
            if (fraction == null)
            {
                return false;
            }

            return ContainsDigits(fraction);
        }
    }
}
