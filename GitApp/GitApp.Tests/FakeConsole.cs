using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitApp.Tests
{
    public class FakeConsole : IConsoleWrapper
    {
        private readonly ConsoleKeyInfo consoleKey;

        public FakeConsole(ConsoleKeyInfo consoleKey)
        {
            this.consoleKey = consoleKey;
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
            return consoleKey;
        }
    }
}
