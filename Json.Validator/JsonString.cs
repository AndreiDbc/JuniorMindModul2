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
                if (input[i] < ' ')
                {
                    return true;
                }
            }

            return false;
        }

        private static bool ContainsRecognisedEscapeCharacters(string input)
        {
            int i = 0;
            do
            {
                if (input[i].Equals('\\') && input[i + 1].Equals('\\'))
                {
                    i++;
                }
                else if (input[i].Equals('\\') && (!VerifyIfContainsRecognisedCharacter(input[i + 1]) || !CanContainUnicode(input.Substring(i + 1))))
                {
                    return false;
                }

                i++;
            }
            while (i < input.Length - 1);

            return !input.EndsWith("\\\"");
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
            return (input.StartsWith("u") && input.Length >= HexNoLength && IsHexNumberValid(input.ToLower())) || !input.StartsWith("u");
        }

        private static bool IsHexNumberValid(string input)
        {
            bool isHexValid = true;
            for (int i = 1; i < HexNoLength + 1 && i < input.Length; i++)
            {
                isHexValid &= CompareCharactersToHexadecimalDigits(input[i]);
            }

            return isHexValid;
        }

        private static bool CompareCharactersToHexadecimalDigits(char digit)
        {
            return char.IsDigit(digit) || (digit >= 'a' && digit <= 'f');
        }
    }
}