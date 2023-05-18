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

        public ConsoleKeyInfo ReadKey()
        {
            return Console.ReadKey();
        }
    }
}
