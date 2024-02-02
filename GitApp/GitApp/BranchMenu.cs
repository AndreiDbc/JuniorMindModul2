using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitApp
{
    public class BranchMenu : Menu
    {
        private readonly GitManager gitManager;
        private readonly string branchesNames;
        private List<string> leftSide;

        public BranchMenu(IConsoleWrapper consoleWrapper, GitManager gitManager, string branchesNames, ModifiedFiles branches) : base(consoleWrapper)
        {
            this.gitManager = gitManager;
            this.branchesNames = branchesNames;
            var height = consoleWrapper.WindowHeight;
            var width = consoleWrapper.WindowWidth;
            var boxHeight = (int)((height - 2) * 0.75);
            var boxWidth = (int)((width - 2) * 0.75);
            leftSide = branches.GetHighlightedSide(boxHeight - 2, (int)(0.4 * boxWidth - 1), (boxHeight - 2) * (int)(0.4 * boxWidth - 1));
            Branches = branches;
            BranchesNames = branchesNames;
        }

        public ModifiedFiles Branches { get; set; }

        public string BranchesNames { get; set; }

        public override void UpdateMenu(bool isWindowToSmall, ConsoleKey key)
        {
            var height = consoleWrapper.WindowHeight;
            var width = consoleWrapper.WindowWidth;
            var boxHeight = (int)(height * 0.75);
            var boxWidth = (int)(width * 0.75);
            List<string> leftSideTemp;
            var commitList = new List<Commit>();
            var rightSide = new List<string>();
            var shouldDisplayOnWholeWindow = height < 15 && width < 60;
            boxHeight = shouldDisplayOnWholeWindow ? height : boxHeight;
            boxWidth = shouldDisplayOnWholeWindow ? width : boxWidth;
            foreach (var branch in branchesNames.Split("\n"))
            {
                commitList.Add(new Commit(gitManager.GetLastCommitMadeInBranch(branch.StartsWith("*") ? branch[2..] : branch)));
                var commit = commitList[^1].GetElement().Split("\n");
                var element = commit[0] + " " + commit[3];
                var elementUtilities = new PageUtilities(element);
                var availableWidth = (int)(0.6 * boxWidth);
                element = element.Length > availableWidth ? elementUtilities.TruncateText(availableWidth) : element;
                rightSide.Add(element + "\x1b[0m");
            }

            var keyInteractor = new KeyInteractor<IElement>(Branches);
            keyInteractor.KeyInfo = key;
            (Branches.Index, Branches.Current) = keyInteractor
                .KeySwitch(leftSide, leftSide[^1], leftSide[0], false, (int)(0.4 * boxWidth) - 2, Branches[Branches.Current].GetElement().Length);
            leftSide = Branches.GetHighlightedSide(boxHeight - 3, (int)(0.4 * boxWidth) - 2, (boxHeight - 2) * (int)(0.4 * boxWidth) - 2);
            leftSideTemp = new PageUtilities(leftSide.ToArray()).PadElement((int)(0.4 * boxWidth) - 2);
            LastPageDisplayed = string.Join(
                Environment.NewLine,
                new BoxDrawer(new ScreenSelector(leftSideTemp, rightSide, consoleWrapper).MergeSides(), "Branches").DrawBox(boxWidth, boxHeight, false, true));
        }
    }
}
