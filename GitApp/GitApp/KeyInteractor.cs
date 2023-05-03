using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitApp
{
    public class KeyInteractor
    {
        private readonly FullCommits commits;

        public KeyInteractor(FullCommits commits)
        {
            this.commits = commits;
        }

        public ConsoleKey KeyInfo { get; set; }

        public (int index, int current) KeySwitch(List<string> page, bool isPageLargerThanWindow)
        {
            return KeyInfo switch
            {
                ConsoleKey.DownArrow => InteractWithDownArrow(page, isPageLargerThanWindow),
                ConsoleKey.UpArrow => InteractWithUpArrow(page),
                ConsoleKey.PageDown => InteractWithPageDown(page),
                ConsoleKey.PageUp => InteractWithPageUp(page),
                _ => (commits.Index, commits.Current)
            };
        }

        private static int SetToZeroOrDontChange(int number)
        {
            return number < 0 ? 0 : number;
        }

        private (int index, int current) InteractWithPageUp(List<IElement> page)
        {
            return (SetToZeroOrDontChange(text.Index - page.Count), SetToZeroOrDontChange(text.Current - page.Count));
        }

        private (int index, int current) InteractWithDownArrow(List<IElement> page, bool isPageLargerThanWindow)
        {
            var lastPageElement = page[^1];
            var list = text.GetText();
            var currentElement = list[text.Current];

            if (currentElement.Equals(lastPageElement) && currentElement != list[^1])
            {
                text.Index++;
                text.Index = isPageLargerThanWindow
                    ? text.Index + list[text.Current].GetElement().Length / Console.WindowWidth
                    : text.Index;
            }

            return (SetToLastOrDontChange(text.Index), SetToLastOrDontChange(text.Current + 1));
        }

        private (int index, int current) InteractWithUpArrow(List<IElement> page)
        {
            var list = text.GetText();
            var firstElement = page[0];
            if (list[text.Current].Equals(firstElement))
            {
                text.Index--;
            }

            return (SetToZeroOrDontChange(text.Index), SetToZeroOrDontChange(text.Current - 1));
        }

        private (int index, int current) InteractWithPageDown(List<IElement> page)
        {
            var list = text.GetText();
            text.Current = SetToLastOrDontChange(text.Current + page.Count);
            var lastElement = page[^1];
            if (lastElement.Equals(list[^1]))
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