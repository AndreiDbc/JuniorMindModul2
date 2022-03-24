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

            return IsValidInteger(input, dotIndex, exponentIndex);
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
    }
}
