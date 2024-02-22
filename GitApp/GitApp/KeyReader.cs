using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitApp
{
    public class KeyReader
    {
        private readonly IConsoleWrapper consoleWrapper;

        public KeyReader(IConsoleWrapper consoleWrapper)
        {
            this.consoleWrapper = consoleWrapper;
        }

        public ConsoleKeyInfo SetKeyValue(int windowHeight, int windowWidth, bool intercept = false)
        {
            while (true)
            {
                if (consoleWrapper.KeyAvailable())
                {
                    return consoleWrapper.ReadKey(intercept);
                }
                else if (windowHeight != consoleWrapper.WindowHeight || windowWidth != consoleWrapper.WindowWidth)
                {
                    return new ConsoleKeyInfo('\0', ConsoleKey.NoName, false, false, false);
                }

                Thread.Sleep(100);
            }
        }
    }
}
