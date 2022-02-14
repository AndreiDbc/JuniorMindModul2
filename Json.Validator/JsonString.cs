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
            isValid &= DoesNotContainControlCharacters(input);
            isValid &= ContainsRecognisedEscapeCharacters(input);
            return isValid;
        }

        private static bool HasStartAndEndQuotes(string input)
        {
            if (input.StartsWith("\"") && input.EndsWith("\"") && input.Length != 1)
            {
                return true;
            }
            else if (input.StartsWith("\"") || input.EndsWith("\""))
            {
                return false;
            }

            return true;
        }

        private static bool DoesNotContainControlCharacters(string input)
        {
            return !input.Contains("\r") || !input.Contains("\n");
        }

        private static bool ContainsRecognisedEscapeCharacters(string input)
        {
            int j = 0;
            int count = 0;
            int countSolidus = 0;
            char[] recognisedCharacters = { 'a', 'b', 't', 'r', 'f', 'n', '\"', '\\', '/', 'u', 'U' };
            for (int i = 0; i < input.Length - 1; i++)
            {
                if (input[i].Equals('\\'))
                {
                    countSolidus++;
                }

                do
                {
                    if (input[i].Equals('\\') && input[i + 1].Equals(recognisedCharacters[j]))
                    {
                        count++;
                    }

                    j++;
                }
                while (j < recognisedCharacters.Length);
                j = 0;
                if (input[i].Equals('\\') && input[i + 1].Equals('\\'))
                {
                    countSolidus--;
                }
            }

            if (count != countSolidus)
            {
                return false;
            }

            return CanContainEscapedCharacters(input);
        }

        private static bool CanContainEscapedCharacters(string input)
        {
            bool isValid = !input.Contains("\"") || input.Contains("\"");
            isValid &= !input.Contains("\\") || input.Contains("\\");
            isValid &= !input.Contains("/") || input.Contains("/");
            isValid &= !input.Contains("\b") || input.Contains("\b");
            isValid &= !input.Contains("\f") || input.Contains("\f");
            isValid &= !input.Contains("\n") || input.Contains("\n");
            isValid &= !input.Contains("\r") || input.Contains("\r");
            isValid &= !input.Contains("\t") || input.Contains("\t");

            return isValid;
        }
    }
}
