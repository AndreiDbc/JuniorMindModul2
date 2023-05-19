using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitApp
{
    public interface IConsoleWrapper
    {
        public int WindowWidth { get; }

        public int WindowHeight { get; }

        public ConsoleKeyInfo ReadKey();

        public void SetCursorPosition(int left, int top);

        public void Clear();

        public void WriteLine(string text);

        public void Write(string text);
    }
}
