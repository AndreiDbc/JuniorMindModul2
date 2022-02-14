using System;

namespace Json
{
    public static class JsonNumber
    {
        const int Ten = 10;
        const int Two = 2;

        public static bool IsJsonNumber(string input)
        {
            bool isValid = true;
            if (!double.TryParse(Convert.ToString(input), out double number) || string.IsNullOrEmpty(input))
            {
                return false;
            }

            isValid &= CanHaveASingleDigit(input);
            isValid &= StartsWithZero(input);
            isValid &= !EndsWithDot(input);
            return isValid;
        }

        private static bool CanHaveASingleDigit(string input)
        {
            return input.Length == 1 || input.Length != 1;
        }

        private static bool StartsWithZero(string input)
        {
            if (input.Length == 1 && input == "0")
            {
                return true;
            }
            else if (input.StartsWith("0") && !input.Contains("."))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private static bool EndsWithDot(string input)
        {
            return input.EndsWith(".");
        }
    }
}
