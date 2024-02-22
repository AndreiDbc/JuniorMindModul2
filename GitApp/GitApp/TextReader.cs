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
        private readonly KeyReader keyReader;
        private int currentWidth;
        private int currentHeight;

        public TextReader(int leftCursorPosition, int topCursorPosition, int width, int height, IConsoleWrapper consoleWrapper, KeyReader keyReader)
        {
            this.leftCursorPosition = leftCursorPosition;
            this.topCursorPosition = topCursorPosition;
            this.width = width;
            this.height = height;
            this.consoleWrapper = consoleWrapper;
            currentWidth = leftCursorPosition;
            currentHeight = topCursorPosition;
            this.keyReader = keyReader;
        }

        public (string text, ConsoleKeyInfo key) GetText(ref string result, ConsoleKeyInfo keyInfo)
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
                AddKeyValue(ref result, keyInfo);
            }

            var windowWidth = consoleWrapper.WindowWidth;
            var windowHeight = consoleWrapper.WindowHeight;
            var windowSize = windowWidth * windowHeight;
            var tempWindowSize = 0;

            do
            {
                tempWindowSize = windowSize;
                keyInfo = keyReader.SetKeyValue(windowHeight, windowWidth, true);
                windowHeight = consoleWrapper.WindowHeight;
                windowWidth = consoleWrapper.WindowWidth;
                windowSize = windowWidth * windowHeight;
                if (tempWindowSize != windowSize)
                {
                    return (result, keyInfo);
                }

                var resetCursorToBegining = result.Length == 0 &&
                    keyInfo.Key == ConsoleKey.Backspace;
                var resetCursorToEnd = result.Length == width * (height - 1) &&
                    keyInfo.Key != ConsoleKey.Backspace;

                if (keyInfo.Key.Equals(ConsoleKey.Enter))
                {
                    return (result, keyInfo);
                }
                else if (keyInfo.Key.Equals(ConsoleKey.Escape))
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
                    AddKeyValue(ref result, keyInfo);
                }
            }
            while (true);

            return ("", keyInfo);
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
