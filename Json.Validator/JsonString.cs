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
            const int requiredCharacters = 4;
            bool isValid = true;
            int index = 0;
            for (int i = 0; i < input.Length - 1; i++)
            {
                string number = "";
                if (input[i] == '\\' && input[i + 1] == 'u')
                {
                    index = i + two;

                    number = GetUnicodeCharacters(input, index);
                    isValid &= IsNumberHexadecimal(number) && number.Length == requiredCharacters + 1;
                }
            }

            return isValid;
        }

        private static string GetUnicodeCharacters(string input, int index)
        {
            string number = " ";
            for (int i = 0; i < input.Length && index < input.Length; i++)
            {
                number += input[index];
                index++;
                if (index == input.Length || !char.IsLetterOrDigit(input[index]))
                {
                    return number.ToLower();
                }
            }

            return number.ToLower();
        }

        private static bool IsNumberHexadecimal(string number)
        {
            bool isValid = true;
            for (int i = 1; i < number.Length && isValid; i++)
            {
                isValid &= false;
                isValid = CompareCharactersToHexadecimalDigits(number, i, isValid);
            }

            return isValid;
        }

        private static bool CompareCharactersToHexadecimalDigits(string number, int index, bool isValid)
        {
            const string hexadecimalDiggits = "abcdef";
            for (int j = 0; j < hexadecimalDiggits.Length; j++)
            {
                if ((char.IsLetter(number[index]) && number[index].Equals(hexadecimalDiggits[j])) || char.IsDigit(number[index]))
                {
                    isValid ^= true;
                    return isValid;
                }
            }

            return isValid;
        }
    }
}