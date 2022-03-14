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
            const int asciiCodeOfLastControlCharacter = 31;
            for (int i = 0; i < input.Length; i++)
            {
                if ((int)input[i] <= asciiCodeOfLastControlCharacter)
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

        private static bool IsHexNumberValid(string input, int index, bool isValid)
        {
            if (index + Five >= input.Length)
            {
                return false;
            }

            isValid &= CompareCharactersToHexadecimalDigits(input, index + Two, isValid);
            isValid &= CompareCharactersToHexadecimalDigits(input, index + Three, isValid);
            isValid &= CompareCharactersToHexadecimalDigits(input, index + Four, isValid);
            isValid &= CompareCharactersToHexadecimalDigits(input, index + Five, isValid);
            if (index + Six >= input.Length)
            {
                return isValid;
            }

            isValid &= !char.IsLetterOrDigit(input[index + Six]);

            return isValid;
        }

        private static bool CompareCharactersToHexadecimalDigits(string input, int index, bool isValid)
        {
            const string hexadecimalDiggits = "abcdefABCDEF";
            for (int i = 0; i < hexadecimalDiggits.Length; i++)
            {
                if (char.IsDigit(input[index]) || input[index].Equals(hexadecimalDiggits[i]))
                {
                    isValid &= true;
                    return isValid;
                }
            }

            return false;
        }
    }
}