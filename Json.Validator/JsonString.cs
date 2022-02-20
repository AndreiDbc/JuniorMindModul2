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
            bool containsRecognisedCharacter = true;
            const string recognisedCharacters = "\"btrfnu/\u005c";
            for (int i = 0; i < input.Length - 1 && input.Contains("\\"); i++)
            {
                containsRecognisedCharacter = VerifyIfContainsRecognisedCharacter(input, i, recognisedCharacters);

                if (!containsRecognisedCharacter)
                {
                    return containsRecognisedCharacter;
                }
            }

            return containsRecognisedCharacter;
        }

        private static bool VerifyIfContainsRecognisedCharacter(string input, int index, string recognisedCharacters)
        {
            for (int i = 0; i < recognisedCharacters.Length; i++)
            {
                if ((input[index].Equals('\\') && input[index + 1].Equals(Convert.ToChar(recognisedCharacters[i]))) || (input[index].Equals('\\') && input[index - 1].Equals('\\')))
                {
                    return true;
                }
            }

            return !input[index].Equals('\\');
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
