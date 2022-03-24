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

            return IsValidInteger(input, dotIndex, exponentIndex) &&
             IsValidFraction(input, dotIndex, exponentIndex) &&
             IsValidExponent(input, exponentIndex);
        }

        private static string GetInteger(string input, int index)
        {
            if (index != -1)
            {
                return input.Substring(0, index - 1);
            }

            return input;
        }

        private static string GetFraction(string input, int dotIndex, int exponentIndex)
        {
            if (dotIndex != -1)
            {
                if (exponentIndex != -1 && dotIndex < exponentIndex)
                {
                    return input.Substring(dotIndex + 1, input.Length - 1 - exponentIndex - 1);
                }

                return input.Substring(dotIndex + 1);
            }

            return string.Empty;
        }

        private static string GetExponent(string input, int exponentIndex)
        {
            if (exponentIndex != -1)
            {
                return input.Substring(exponentIndex + 1);
            }

            return string.Empty;
        }

        private static bool ContainsDigitsOrSigns(string input)
        {
            for (int i = 0; i < input.Length; i++)
            {
                if (!(char.IsDigit(input[i]) || input[i].Equals('-') || input[i].Equals('+')))
                {
                    return false;
                }
            }

            return true;
        }

        private static bool IsValidInteger(string input, int dotIndex, int exponentIndex)
        {
            bool isValidInteger = true;
            isValidInteger &= !(input.StartsWith("0") && !input.Contains(".") && input.Length > 1);
            isValidInteger &= !input.EndsWith(".");
            isValidInteger &= ContainsDigitsOrSigns(GetInteger(input, dotIndex)) || ContainsDigitsOrSigns(GetInteger(input, exponentIndex));

            return isValidInteger;
        }

        private static bool IsValidExponent(string input, int exponentIndex)
        {
            bool isValidExponent = exponentIndex == -1;
            string exponent = GetExponent(input, exponentIndex);
            if (exponentIndex != -1)
            {
                if (string.IsNullOrEmpty(exponent))
                {
                    return false;
                }

                isValidExponent = ContainsDigitsOrSigns(exponent);
                isValidExponent &= !exponent.EndsWith("+") && !exponent.EndsWith("-");
            }

            return isValidExponent;
        }

        private static bool IsValidFraction(string input, int dotIndex, int exponentIndex)
        {
            bool isValidInteger = true;
            string fraction = GetFraction(input, dotIndex, exponentIndex);
            if (!string.IsNullOrEmpty(fraction))
            {
                if (exponentIndex != -1)
                {
                    isValidInteger &= IsValidExponent(input, exponentIndex);
                }

                isValidInteger &= ContainsDigitsOrSigns(fraction);
            }

            return isValidInteger;
        }
    }
}
