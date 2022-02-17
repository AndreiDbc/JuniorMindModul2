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
            isValid &= GetHexadecimalUnicodeNumber(input);
            return isValid;
        }

        private static bool HasStartAndEndQuotes(string input)
        {
            return input.StartsWith("\"") && input.EndsWith("\"") && input.Length != 1;
        }

        private static bool ContainsControlCharacters(string input)
        {
            return input.Contains("\r") || input.Contains("\n");
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

        private static bool GetHexadecimalUnicodeNumber(string input)
        {
            const int two = 2;
            const int minimumCharacters = 4;
            bool containsMinimumCharacters = true;
            int index = 0;
            string temp = input + " ";
            string number = "";
            for (int i = 0; i < temp.Length; i++)
            {
                if (temp[i] == '\\' && (temp[i + 1] == 'u'))
                {
                    index = i + two;

                    if (index >= temp.Length - two)
                    {
                        return false;
                    }

                    do
                    {
                        number += temp[index];
                        index++;
                    }
                    while (char.IsLetterOrDigit(temp[index]));
                    containsMinimumCharacters = number.Length >= minimumCharacters;
                }
            }

            if (string.IsNullOrEmpty(number))
            {
                return true;
            }

            return containsMinimumCharacters;
        }

        private static bool ContainsFinishedHexadecimalUnicodeNumber(string number)
        {
            const int minimumCharacters = 4;
            int countCharacters = 0;
            for (int i = 0; i < number.Length; i++)
            {
                if (countCharacters < minimumCharacters && number[i].Equals(' '))
                {
                    return false;
                }
                else if (number[i].Equals(' '))
                {
                    countCharacters = -1;
                }

                countCharacters++;
            }

            return true;
        }
    }
}
