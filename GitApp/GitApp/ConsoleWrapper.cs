using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitApp
{
    public class ConsoleWrapper : IConsoleWrapper
    {
        public int WindowWidth => Console.WindowWidth;

        public int WindowHeight => Console.WindowHeight;

        public void SetCursorPosition(int left, int top)
        {
            Console.SetCursorPosition(left, top);
        }

        public bool KeyAvailable()
        {
            return Console.KeyAvailable;
        }

        public ConsoleKeyInfo ReadKey()
        {
            return Console.ReadKey();
        }

        public void Clear()
        {
            Console.Clear();
        }

        public void WriteLine(string text)
        {
            Console.WriteLine(text);
        }

        public void Write(string text)
        {
            Console.Write(text);
        }
    }
}
