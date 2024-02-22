using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitApp
{
    public class ConsoleWrapper : IConsoleWrapper
    {
        public int WindowWidth { get => Console.WindowWidth; set => Console.WindowWidth = value; }

        public int WindowHeight { get => Console.WindowHeight; set => Console.WindowHeight = value; }

        public bool CursorVisible { get => Console.CursorVisible; set => Console.CursorVisible = value; }

        public void SetCursorPosition(int left, int top)
        {
            Console.SetCursorPosition(left, top);
        }

        public void SetOutputEncodingToUTF8()
        {
            Console.OutputEncoding = Encoding.UTF8;
        }

        public bool KeyAvailable()
        {
            return Console.KeyAvailable;
        }

        public void Flush()
        {
            Console.Out.Flush();
        }

        public ConsoleKeyInfo ReadKey(bool intercept = false)
        {
            return Console.ReadKey(intercept);
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
