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

        private static int SetToZeroOrDontChange(int number)
        {
            return number < 0 ? 0 : number;
        }

        private (int index, int current) InteractWithPageUp(List<string> page)
        {
            return (SetToZeroOrDontChange(text.Index - page.Count), SetToZeroOrDontChange(text.Current - page.Count));
        }

        private (int index, int current) InteractWithDownArrow(List<string> page, bool isPageLargerThanWindow)
        {
            var lastPageElement = page[^1];
            var list = text.GetText();
            var currentElement = list[text.Current];

            if (currentElement == lastPageElement)
            {
                text.Index++;
                text.Index = isPageLargerThanWindow
                    ? text.Index + list[text.Current].Length / Console.WindowWidth
                    : text.Index;
            }

            return (SetToLastOrDontChange(text.Index), SetToLastOrDontChange(text.Current + 1));
        }

        private (int index, int current) InterractWithUpArrow(List<string> page)
        {
            var list = text.GetText();
            var firstElement = page[0];
            if (list[text.Current] == firstElement)
            {
                text.Index--;
            }

            return (SetToZeroOrDontChange(text.Index), SetToZeroOrDontChange(text.Current - 1));
        }
    }
}
