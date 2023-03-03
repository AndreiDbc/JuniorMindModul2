using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitApp
{
    public class KeyInteractor
    {
        private readonly ConsoleKey keyInfo;
        private readonly IText text;

        public KeyInteractor(IText text, ConsoleKey keyInfo)
        {
            this.keyInfo = keyInfo;
            this.text = text;
        }

        public (int index, int current) KeySwitch(List<string> page, bool isPageLargerThanWindow)
        {
            return keyInfo switch
            {
                ConsoleKey.DownArrow => InteractWithDownArrow(page, isPageLargerThanWindow),
                ConsoleKey.UpArrow => InteractWithUpArrow(page),
                ConsoleKey.PageDown => InteractWithPageDown(page),
                ConsoleKey.PageUp => InteractWithPageUp(page),
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

        private (int index, int current) InteractWithUpArrow(List<string> page)
        {
            var list = text.GetText();
            var firstElement = page[0];
            if (list[text.Current] == firstElement)
            {
                text.Index--;
            }

            return (SetToZeroOrDontChange(text.Index), SetToZeroOrDontChange(text.Current - 1));
        }

        private (int index, int current) InteractWithPageDown(List<string> page)
        {
            var list = text.GetText();
            text.Current = SetToLastOrDontChange(text.Current + page.Count);
            var lastElement = page[^1];
            if (lastElement == list[^1])
            {
                return (text.Index, text.Current);
            }

            return (SetToLastOrDontChange(text.Index + page.Count), text.Current);
        }

        private int SetToLastOrDontChange(int number)
        {
            var list = text.GetText();
            return number > list.Count - 1
                ? list.Count - 1
                : number;
        }
    }
}
