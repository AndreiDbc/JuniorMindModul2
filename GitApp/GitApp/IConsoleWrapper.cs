using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitApp
{
    public interface IConsoleWrapper
    {
        public int WindowWidth { get; set; }

        public int WindowHeight { get; set;  }

        public ConsoleKeyInfo ReadKey(bool intercept = false);

        public void SetCursorPosition(int left, int top);

        public void SetOutputEncodingToUTF8();

        public void Flush();

        public void Clear();

        public void WriteLine(string text);

        public void Write(string text);

        public bool KeyAvailable();
    }
}
