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

        public (int index, int current) KeySwitch(List<string> page, string lastCommitIdOnPage, bool isPageLargerThanWindow, int consoleWidth)
        {
            return KeyInfo switch
            {
                ConsoleKey.DownArrow => InteractWithDownArrow(lastCommitIdOnPage, isPageLargerThanWindow, consoleWidth),
                ConsoleKey.UpArrow => InteractWithUpArrow(lastCommitIdOnPage),
                ConsoleKey.PageDown => InteractWithPageDown(page, lastCommitIdOnPage),
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

        private (int index, int current) InteractWithDownArrow(string lastCommitIdOnPage, bool isPageLargerThanWindow, int consoleWidth)
        {
            var list = commits.GetText();
            var currentId = list[commits.Current].Id;

            if (currentId.Equals(lastCommitIdOnPage) && currentId != list[^1].Id)
            {
                commits.Index++;
                commits.Index = isPageLargerThanWindow
                    ? commits.Index + list[commits.Current].GetElement().Length / consoleWidth
                    : commits.Index;
            }

            return (SetToLastOrDontChange(commits.Index), SetToLastOrDontChange(commits.Current + 1));
        }

        private (int index, int current) InteractWithUpArrow(string id)
        {
            var list = commits.GetText();
            if (list[commits.Current].Id.Equals(id))
            {
                commits.Index--;
            }

            return (SetToZeroOrDontChange(commits.Index), SetToZeroOrDontChange(commits.Current - 1));
        }

        private (int index, int current) InteractWithPageDown(List<string> page, string lastCommitIdOnPage)
        {
            var list = commits.GetText();
            commits.Current = SetToLastOrDontChange(commits.Current + page.Count);
            if (lastCommitIdOnPage.Equals(list[^1].Id))
            {
                return (commits.Index, commits.Current);
            }

            return (SetToLastOrDontChange(commits.Index + page.Count), commits.Current);
        }

        private int SetToLastOrDontChange(int number)
        {
            var list = commits.GetText();
            return number > list.Count - 1
                ? list.Count - 1
                : number;
        }
    }
}