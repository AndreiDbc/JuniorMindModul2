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

        public (int index, int current) KeySwitch(
            List<string> page,
            string lastCommitIdOnPage,
            string firstCommitOnPage,
            bool isPageLargerThanWindow,
            int consoleWidth)
        {
            return KeyInfo switch
            {
                ConsoleKey.DownArrow => InteractWithDownArrow(lastCommitIdOnPage, isPageLargerThanWindow, consoleWidth),
                ConsoleKey.UpArrow => InteractWithUpArrow(firstCommitOnPage),
                ConsoleKey.PageDown => InteractWithPageDown(page, lastCommitIdOnPage),
                ConsoleKey.PageUp => InteractWithPageUp(page),
                _ => (commits.Index, commits.Current)
            };
        }

        private (int index, int current) InteractWithPageUp(List<string> page)
        {
            return (Math.Max(0, commits.Index - page.Count), Math.Max(0, commits.Current - page.Count));
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

            return (Math.Min(list.Count - 1, commits.Index), Math.Min(list.Count - 1, ++commits.Current));
        }

        private (int index, int current) InteractWithUpArrow(string firstCommitOnPage)
        {
            var list = commits.GetText();
            if (list[commits.Current].Id.Equals(firstCommitOnPage))
            {
                commits.Index--;
            }

            return (Math.Max(0, commits.Index), Math.Max(0, --commits.Current));
        }

        private (int index, int current) InteractWithPageDown(List<string> page, string lastCommitIdOnPage)
        {
            var list = commits.GetText();
            commits.Current = Math.Min(list.Count - 1, commits.Current + page.Count);
            if (lastCommitIdOnPage.Equals(list[^1].Id))
            {
                return (commits.Index, commits.Current);
            }

            return (Math.Min(list.Count - 1, commits.Index + page.Count), commits.Current);
        }
    }
}