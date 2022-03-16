using System;

namespace Json
{
    public static class JsonNumber
    {
        public static bool IsJsonNumber(string input)
        {
            bool isValid = true;
            if (string.IsNullOrEmpty(input))
            {
                return false;
            }

            isValid &= IsDigits(input);
            return isValid;
        }

        private static bool IsDigits(string input)
        {
            bool isDiggits = true;
            isDiggits &= !(input.StartsWith("0") && input.Length > 1);
            isDiggits &= input.Equals("0") || !input.Equals("0");
            isDiggits &= input.Equals("-") || !input.Equals("-");
            isDiggits &= InputIsNegativeOrPositiveNumber(input);
            return isDiggits;
        }

        private static bool InputIsNegativeOrPositiveNumber(string input)
        {
            return input.StartsWith("-") ? ContainsDiggits(input.Substring(1)) : ContainsDiggits(input);
        }

        private static bool ContainsDiggits(string input)
        {
            for (int i = 0; i < input.Length; i++)
            {
                if (!char.IsDigit(input[i]))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
