using System;

namespace Json
{
    public static class JsonString
    {
        const int HexNoLength = 4;

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
            isValid &= CanContainUnicode(input);
            return isValid;
        }

        private static bool HasStartAndEndQuotes(string input)
        {
            return input.StartsWith("\"") && input.EndsWith("\"") && input.Length != 1;
        }

        private static bool ContainsControlCharacters(string input)
        {
            for (int i = 0; i < input.Length; i++)
            {
                if ((int)input[i] < (int)' ')
                {
                    return true;
                }
            }

            return false;
        }

        private static bool ContainsRecognisedEscapeCharacters(string input)
        {
            int i = 0;
            bool containsRecognisedCharacter = !input.EndsWith("\\\"");
            do
            {
                if (input[i].Equals('\\') && input[i + 1].Equals('\\'))
                {
                    i++;
                }
                else if (input[i].Equals('\\'))
                {
                    containsRecognisedCharacter &= VerifyIfContainsRecognisedCharacter(input[i + 1]);
                    containsRecognisedCharacter &= CanContainUnicode(input.Substring(i + 1));
                }

                i++;
            }
            while (i < input.Length - 1 && containsRecognisedCharacter);

            return containsRecognisedCharacter;
        }

        private static bool VerifyIfContainsRecognisedCharacter(char character)
        {
            const string recognisedCharacters = "\"btrfnu/\\";
            for (int i = 0; i < recognisedCharacters.Length; i++)
            {
                if (character.Equals(recognisedCharacters[i]))
                {
                    return true;
                }
            }

            return false;
        }

        private static bool CanContainUnicode(string input)
        {
            bool isValid = true;
            if (input[0] != 'u')
            {
                isValid = true;
            }
            else if (input.Length < HexNoLength + 1)
            {
                isValid = false;
            }
            else
            {
                isValid &= IsHexNumberValid(input.ToLower());
            }

            return isValid;
        }

        private static bool IsHexNumberValid(string input)
        {
            bool isHexValid = true;
            for (int i = 1; i < HexNoLength + 1 && isHexValid; i++)
            {
                isHexValid &= CompareCharactersToHexadecimalDigits(input[i]);
                if (i == input.Length - 1)
                {
                    return isHexValid;
                }
            }

            return isHexValid && !char.IsLetterOrDigit(input[HexNoLength + 1]);
        }

        private static bool CompareCharactersToHexadecimalDigits(char digit)
        {
            return char.IsDigit(digit) || ((int)digit >= (int)'a' && (int)digit <= 'f');
        }
    }
}