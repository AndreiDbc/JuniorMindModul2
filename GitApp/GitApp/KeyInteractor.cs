using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GitApp
{
    public class KeyInteractor<T>
        where T : IElement
    {
        private readonly IInteractor<T> interactor;

        public KeyInteractor(IInteractor<T> interactor)
        {
            this.interactor = interactor;
        }

        public ConsoleKey KeyInfo { get; set; }

        public (int index, int current) KeySwitch(
            List<string> page,
            string lastCommitIdOnPage,
            string firstCommitOnPage,
            bool isPageLargerThanWindow,
            int consoleWidth,
            int currentElementLength)
        {
            return KeyInfo switch
            {
                ConsoleKey.DownArrow => InteractWithDownArrow(lastCommitIdOnPage, isPageLargerThanWindow, consoleWidth, currentElementLength),
                ConsoleKey.UpArrow => InteractWithUpArrow(firstCommitOnPage),
                ConsoleKey.PageDown => InteractWithPageDown(page, lastCommitIdOnPage),
                ConsoleKey.PageUp => InteractWithPageUp(page),
                _ => (interactor.Index, interactor.Current)
            };
        }

        private (int index, int current) InteractWithPageUp(List<string> page)
        {
            return (Math.Max(0, interactor.Index - page.Count), Math.Max(0, interactor.Current - page.Count));
        }

        private (int index, int current) InteractWithDownArrow(string lastCommitIdOnPage, bool isPageLargerThanWindow, int consoleWidth, int currentElementLength)
        {
            var list = interactor.GetText();
            var currentId = list[Math.Min(interactor.Current, list.Count - 1)].Id;
            interactor.Current++;
            if ((currentId.Equals(lastCommitIdOnPage) && currentId != list[^1].Id) || (currentId.Equals(lastCommitIdOnPage) && isPageLargerThanWindow))
            {
                interactor.Index++;
                interactor.Index = isPageLargerThanWindow ? interactor.Index + currentElementLength / consoleWidth : interactor.Index;
            }

            return (Math.Min(list.Count - 1, interactor.Index), Math.Min(list.Count - 1, interactor.Current));
        }

        private (int index, int current) InteractWithUpArrow(string firstCommitOnPage)
        {
            var list = interactor.GetText();
            if (list[interactor.Current].Id.Equals(firstCommitOnPage))
            {
                interactor.Index--;
            }

            return (Math.Max(0, interactor.Index), Math.Max(0, --interactor.Current));
        }

        private (int index, int current) InteractWithPageDown(List<string> page, string lastCommitIdOnPage)
        {
            var list = interactor.GetText();
            interactor.Current = Math.Min(list.Count - 1, interactor.Current + page.Count);
            if (lastCommitIdOnPage.Equals(list[^1].Id))
            {
                return (interactor.Index, interactor.Current);
            }

            return (Math.Min(list.Count - 1, interactor.Index + page.Count), interactor.Current);
        }
    }
}