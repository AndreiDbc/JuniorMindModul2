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

            return GetJsonNumberValidity(input, dotIndex, exponentIndex);
        }

        private static bool GetJsonNumberValidity(string input, int dotIndex, int exponentIndex)
        {
            bool isValid = IsValidInteger(input, dotIndex, exponentIndex);
            isValid &= IsValidFraction(input, dotIndex, exponentIndex);
            isValid &= IsValidExponent(input, dotIndex, exponentIndex);
            return isValid;
        }

        private static bool IsValidInteger(string input, int dotIndex, int exponentIndex)
        {
            bool isValidInteger = true;
            isValidInteger &= !(input.StartsWith("0") && !input.Contains(".") && input.Length > 1);
            isValidInteger &= !input.EndsWith(".");
            if (dotIndex != -1 || exponentIndex != -1)
            {
                isValidInteger &= ContainsDigitsOrSigns(GetNumberPart(input, dotIndex)) || ContainsDigitsOrSigns(GetNumberPart(input, exponentIndex));
            }

            return isValidInteger;
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

        private static string GetNumberPart(string input, int index)
        {
            if (index != -1)
            {
                return input.Substring(index + 1);
            }

            return input;
        }

        private static bool IsValidExponent(string input, int dotIndex, int exponentIndex)
        {
            bool isValidExponent = exponentIndex == -1;
            if (!string.IsNullOrEmpty(GetNumberPart(input, exponentIndex)) && exponentIndex != -1 && dotIndex < exponentIndex)
            {
                isValidExponent = ContainsDigitsOrSigns(input.Substring(dotIndex + 1, input.Length - 1 - exponentIndex - 1));
                isValidExponent &= !(GetNumberPart(input, exponentIndex).EndsWith("+") || GetNumberPart(input, exponentIndex).EndsWith("-"));
            }

            return isValidExponent;
        }

        private static bool IsValidFraction(string input, int dotIndex, int exponentIndex)
        {
            bool isValidInteger = true;
            if (!string.IsNullOrEmpty(GetNumberPart(input, dotIndex)))
            {
                if (exponentIndex != -1 && dotIndex < exponentIndex)
                {
                    isValidInteger &= IsValidExponent(input, dotIndex, exponentIndex);
                    return isValidInteger;
                }

                isValidInteger &= ContainsDigitsOrSigns(GetNumberPart(input, dotIndex));
            }

            return isValidInteger;
        }
    }
}
