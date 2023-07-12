using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitApp.Tests
{
    public class FakeConsole : IConsoleWrapper
    {
        private ConsoleKeyInfo[] keySequence;
        private ConsoleKeyInfo lastPressedKey;
        private int currentKeyIndex;
        public StringWriter StringWriter;
        public string lastFrame;
        private bool keyAvailable;

        public FakeConsole(ConsoleKeyInfo[] keySequence, StringWriter stringWriter)
        {
            this.keySequence = keySequence;
            lastPressedKey = new ConsoleKeyInfo((char)ConsoleKey.Escape, ConsoleKey.Escape, false, false, false);
            currentKeyIndex = 0;
            StringWriter = stringWriter;
        }

        public bool KeyAvailable()
        {
            return keyAvailable;
        }

        public int WindowWidth { get; set; }

        public int WindowHeight { get; set; }

        public int SetCursorPositionLeft { get; private set; }

        public int SetCursorPositionTop { get; private set; }

        public void SetCursorPosition(int left, int top)
        {
            SetCursorPositionLeft = left; 
            SetCursorPositionTop = top; 
        }

        public ConsoleKeyInfo ReadKey()
        {
            currentKeyIndex++;
            if (currentKeyIndex > keySequence.Length)
            {
                return lastPressedKey;
            }

            return keySequence[currentKeyIndex - 1];
        }

        public void Write(string text)
        {
        }

        public void WriteLine(string text)
        {
        }

        public void Clear()
        {
            lastFrame += StringWriter.ToString();
        }
    }
}
