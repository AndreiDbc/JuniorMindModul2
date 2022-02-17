using System;

namespace Json
{
    public static class JsonString
    {
        public static bool IsJsonString(string input)
        {
            bool isValid = true;
            if (string.IsNullOrEmpty(input))
            {
                return false;
            }

            isValid &= HasStartAndEndQuotes(input);
            isValid &= !ContainsControlCharacters(input);
            isValid &= ContainsRecognisedEscapeCharacters(input);
            isValid &= !EndsWithReverseSolidus(input);
            isValid &= EndsWithFinishedHexNumber(input);
            return isValid;
        }

        private static bool HasStartAndEndQuotes(string input)
        {
            return input.StartsWith("\"") && input.EndsWith("\"") && input.Length != 1;
        }

        private static bool ContainsControlCharacters(string input)
        {
            bool isValid = input.Contains("\r");
            isValid &= input.Contains("\n");
            return isValid;
        }

        private static bool ContainsRecognisedEscapeCharacters(string input)
        {
            int j = 0;
            bool containsRecognisedCharacter = true;
            string[] recognisedCharacters = { "a", "b", "t", "r", "f", "n", "\"", "\\", "/", "u" };
            for (int i = 0; i < input.Length - 1; i++)
            {
                if (input[i].Equals('\\') && input[i - 1].Equals('\\'))
                {
                    containsRecognisedCharacter = true;
                }
                else if (input[i].Equals('\\'))
                {
                    containsRecognisedCharacter = false;
                }

                do
                {
                    if (input[i].Equals('\\') && input[i + 1].Equals(Convert.ToChar(recognisedCharacters[j])))
                    {
                        containsRecognisedCharacter = true;
                        break;
                    }

                    j++;
                }
                while (j < recognisedCharacters.Length);
                if (!containsRecognisedCharacter)
                {
                    return containsRecognisedCharacter;
                }

                j = 0;
            }

            return containsRecognisedCharacter;
        }

        private static bool EndsWithReverseSolidus(string input)
        {
            return input.Length > 1 && input[input.Length - 1 - 1].Equals('\\');
        }

        private static bool EndsWithFinishedHexNumber(string input)
        {
            const int two = 2;
            const int minimumCharacters = 4;
            bool containsMinimumCharacters = true;
            int index = 0;
            string number = "";
            for (int i = 0; i < input.Length - 1; i++)
            {
                if (input[i] == '\\' && (input[i + 1] == 'u'))
                {
                    index = i + two;

                    if (index >= input.Length - 1)
                    {
                        return false;
                    }

                    do
                    {
                        number += input[index];
                        index++;
                    }
                    while (char.IsLetterOrDigit(input[index]));
                    containsMinimumCharacters = number.Length >= minimumCharacters;
                }
            }

            if (string.IsNullOrEmpty(number))
            {
                return true;
            }

            return containsMinimumCharacters;
        }
    }
}
