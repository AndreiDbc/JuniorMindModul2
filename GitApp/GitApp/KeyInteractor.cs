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

        private static string RemoveFormatingCodes(string element)
        {
            var result = element;
            foreach (var code in new[] { "\x1b[0m", "\x1b[7m", "\x1b[48;5;27m", "\x1b[48;5;46m", "\x1b[48;5;196m", "\u001b[38;5;27m", "\u001b[38;5;46m", "\u001b[38;5;196m" })
            {
                result = result.Replace(code, "");
            }

            return result;
        }

        private (int index, int current) InteractWithPageUp(List<string> page)
        {
            return (SetToZeroOrDontChange(commits.Index - page.Count), SetToZeroOrDontChange(commits.Current - page.Count));
        }

        private (int index, int current) InteractWithDownArrow(List<string> page, bool isPageLargerThanWindow)
        {
            var lastPageElement = RemoveFormatingCodes(page[^1]);
            var list = commits.GetText();
            var currentId = list[commits.Current].Id;

            if (currentId.Equals(lastPageElement[..7]) && currentId != list[^1].Id)
            {
                commits.Index++;
                commits.Index = isPageLargerThanWindow
                    ? commits.Index + list[commits.Current].GetElement().Length / Console.WindowWidth
                    : commits.Index;
            }

            return (SetToLastOrDontChange(commits.Index), SetToLastOrDontChange(commits.Current + 1));
        } return (SetToLastOrDontChange(text.Index), SetToLastOrDontChange(text.Current + 1));
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