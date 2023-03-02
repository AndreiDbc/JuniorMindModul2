using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitApp
{
    public class KeyInteractor
    {
        private readonly ConsoleKeyInfo keyInfo;
        private readonly IText text;

        public KeyInteractor(IText text)
        {
            this.keyInfo = Console.ReadKey();
            this.text = text;
        }

        public ConsoleKey Key { get => keyInfo.Key; }

        public (int index, int current) KeySwitch(List<string> page, bool isPageLargerThanWindow)
        {
            return keyInfo.Key switch
            {
                ConsoleKey.DownArrow => InterractWithDownArrow(page, isPageLargerThanWindow),
                ConsoleKey.UpArrow => InterractWithUpArrow(page),
                ConsoleKey.PageDown => InterractWithPageDown(page),
                ConsoleKey.PageUp => InterractWithPageUp(page),
                _ => (text.Index, text.Current)
            };
        }
    }
}
