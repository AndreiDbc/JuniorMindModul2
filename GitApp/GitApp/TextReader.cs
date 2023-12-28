using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitApp
{
    public class TextReader
    {
        private readonly int leftCursorPosition;
        private readonly int topCursorPosition;
        private readonly int width;
        private readonly int height;
        private readonly IConsoleWrapper consoleWrapper;
        private int currentWidth;
        private int currentHeight;

        public TextReader(int leftCursorPosition, int topCursorPosition, int width, int height, IConsoleWrapper consoleWrapper)
        {
            this.leftCursorPosition = leftCursorPosition;
            this.topCursorPosition = topCursorPosition;
            this.width = width;
            this.height = height;
            this.consoleWrapper = consoleWrapper;
            currentWidth = leftCursorPosition;
            currentHeight = topCursorPosition;
        }

        public (string text, ConsoleKeyInfo key) GetText(ref string result, ConsoleKeyInfo key)
        {
            consoleWrapper.SetCursorPosition(leftCursorPosition, topCursorPosition);
            consoleWrapper.Write("\x1b[0m");

            if (result != "")
            {
                new PageUtilities(result).SplitRow(width).ForEach(line =>
                {
                    consoleWrapper.Write(line);
                    currentHeight++;
                    consoleWrapper.SetCursorPosition(leftCursorPosition, currentHeight);
                });
                currentHeight--;
                currentWidth = leftCursorPosition + result.Length % width;
                AddKeyValue(ref result, key);
            }

            var windowSize = consoleWrapper.WindowWidth * consoleWrapper.WindowHeight;
            var tempWindowSize = 0;
            do
            {
                tempWindowSize = windowSize;
                key = consoleWrapper.ReadKey(true);
                windowSize = consoleWrapper.WindowWidth * consoleWrapper.WindowHeight;
                if (tempWindowSize != windowSize)
                {
                    return (result, key);
                }

                var resetCursorToBegining = result.Length == 0 &&
                    key.Key == ConsoleKey.Backspace;
                var resetCursorToEnd = result.Length == width * (height - 1) &&
                    key.Key != ConsoleKey.Backspace;

                if (key.Equals(ConsoleKey.Enter))
                {
                    return (result, key);
                }
                else if (key.Equals(ConsoleKey.Escape))
                {
                    break;
                }

                if (resetCursorToBegining)
                {
                    consoleWrapper.SetCursorPosition(leftCursorPosition, topCursorPosition);
                }
                else if (resetCursorToEnd)
                {
                    consoleWrapper.SetCursorPosition(leftCursorPosition + width, height + topCursorPosition);
                }
                else
                {
                    AddKeyValue(ref result, key);
                }
            }
            while (true);

            return ("", key);
        }

        private void AddKeyValue(ref string result, ConsoleKeyInfo key)
        {
            var keyToChar = key.KeyChar;
            if (key.Key.Equals(ConsoleKey.Backspace))
            {
                result = result[..^1];
                if (currentWidth == leftCursorPosition)
                {
                    consoleWrapper.SetCursorPosition(currentWidth, currentHeight);
                    currentWidth = leftCursorPosition + width - 1;
                    currentHeight--;
                    consoleWrapper.SetCursorPosition(currentWidth, currentHeight);
                    consoleWrapper.Write(" \b");
                }
                else
                {
                    consoleWrapper.SetCursorPosition(currentWidth, currentHeight);
                    currentWidth--;
                    consoleWrapper.SetCursorPosition(currentWidth, currentHeight);
                    consoleWrapper.Write(" \b");
                }
            }
            else if (keyToChar >= ' ')
            {
                consoleWrapper.SetCursorPosition(currentWidth, currentHeight);
                result += keyToChar.ToString();
                consoleWrapper.Write(result[^1].ToString());
                currentWidth = currentWidth - leftCursorPosition < width - 1 ? currentWidth + 1 : leftCursorPosition;
                currentHeight = currentWidth == leftCursorPosition ? currentHeight + 1 : currentHeight;
                consoleWrapper.SetCursorPosition(
                    currentHeight == height + topCursorPosition + 1 ? leftCursorPosition + width - 1 : currentWidth,
                    Math.Min(height + topCursorPosition, currentHeight));
            }
        }
    }
}
