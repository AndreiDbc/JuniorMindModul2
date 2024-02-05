using Castle.Components.DictionaryAdapter.Xml;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Reflection.PortableExecutable;
using System.Runtime.ExceptionServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace GitApp
{
    public class DisplayText
    {
        private const string MenuBar = "\u001b[0mStatus [1] | Log [2]";
        private readonly GitManager gitManager;
        private readonly IConsoleWrapper consoleWrapper;
        private readonly ConsoleKey[] menuKeys;
        private FullCommits commits;
        private TextWriter writer;
        ConsoleKey key;

        public DisplayText(FullCommits commits, TextWriter writer, GitManager gitManager, IConsoleWrapper consoleWrapper)
        {
            this.commits = commits;
            this.writer = writer;
            this.gitManager = gitManager;
            this.consoleWrapper = consoleWrapper;
            IsRightSideInUse = false;
            WasWindowResizedSmaller = false;
            menuKeys = new[] { ConsoleKey.D1, ConsoleKey.D2 };
            key = ConsoleKey.NoName;
        }

        private bool WasWindowResizedSmaller { get; set; }

        private bool IsRightSideInUse { get; set; }

        public void Display()
        {
            DisplayStatus();
            do
            {
                if (key.Equals(ConsoleKey.D1))
                {
                    DisplayStatus();
                }
                else if (key.Equals(ConsoleKey.D2))
                {
                    DisplayLogMenu();
                }
            }
            while (key != ConsoleKey.Escape);
        }

        private static List<string> MergeSides(List<string> leftSide, List<string> rightSide, bool overlayPage = false)
        {
            var result = new List<string>();

            for (int i = 0; i < leftSide.Count; i++)
            {
                result.Add(i < rightSide.Count ? leftSide[i] : leftSide[i] + (overlayPage ? "" : "\n"));
                if (i < rightSide.Count)
                {
                    result[i] += rightSide[i];
                }
            }

            return result;
        }

        private static string SetMenuBarForStatus(bool unstagedAvailability)
        {
            return "\u001b[32m[Esc] Exit \u001b[33m[→] Next menu \u001b[36m[B] Branches "
                + (unstagedAvailability ? "\u001b[34m[⏎] Stage file \u001b[35m[A] Stage all" : "\u001b[34m[⏎] Unstage file \u001b[35m[A] Unstage all\u001b[0m");
        }

        private bool GetExitConditionForHighlightedHunksPage(IElement currentUnstaged, IElement diffElement)
        {
            var exitCondition = key.Equals(ConsoleKey.Escape);
            exitCondition = exitCondition || menuKeys.Contains(key);
            exitCondition = exitCondition || key.Equals(ConsoleKey.LeftArrow);
            exitCondition = exitCondition || currentUnstaged.GetElement() == null;
            exitCondition = exitCondition || key.Equals(ConsoleKey.A);
            exitCondition = exitCondition || diffElement.GetElement() == "";
            return exitCondition;
        }

        private void DisplayLogMenu()
        {
            var windowWidth = consoleWrapper.WindowWidth;
            var windowHeight = consoleWrapper.WindowHeight;
            var windowSize = windowHeight * windowWidth;
            var isWindowToSmall = windowHeight < 5 && windowWidth < 35;
            var menu = new LogMenu(consoleWrapper, commits);
            menu.UpdateMenu(isWindowToSmall, ConsoleKey.NoName);
            WasWindowResizedSmaller = new MenuWriter(menu.LastPageDisplayed, isWindowToSmall, consoleWrapper).UpdateDisplay(WasWindowResizedSmaller);
            var windowSizeTemp = windowSize;

            do
            {
                key = consoleWrapper.ReadKey(true).Key;
                menu.UpdateMenu(isWindowToSmall, key);
                if (key.Equals(ConsoleKey.Escape))
                {
                    break;
                }

                if (key.Equals(ConsoleKey.Enter))
                {
                    DisplayCommitDetailsMenu();
                }
                else if (key.Equals(ConsoleKey.B))
                {
                    DisplayBranchesMenu(menu, isWindowToSmall);
                }
                else if (menu.WasInteractorModified)
                {
                    continue;
                }

                if (menuKeys.Contains(key))
                {
                    break;
                }

                windowWidth = consoleWrapper.WindowWidth;
                windowSize = consoleWrapper.WindowHeight * windowWidth;
                WasWindowResizedSmaller = windowSize < windowSizeTemp;
                windowSizeTemp = windowSize;
                isWindowToSmall = consoleWrapper.WindowHeight < 3;
                WasWindowResizedSmaller = new MenuWriter(menu.LastPageDisplayed, isWindowToSmall, consoleWrapper).UpdateDisplay(WasWindowResizedSmaller);
            }
            while (true);
        }

        private void DisplayStatus()
        {
            var unstaged = new ModifiedFiles(gitManager.GetUnstagedFiles());
            var staged = new ModifiedFiles(gitManager.GetStagedFiles());
            var windowWidth = consoleWrapper.WindowWidth;
            var windowHeight = consoleWrapper.WindowHeight;
            var windowSize = windowHeight * windowWidth;
            var isWindowToSmall = windowHeight < 8 && windowWidth < 35;
            var unstagedAvailability = true;
            var menu = new StatusMenu(consoleWrapper, gitManager, unstaged, staged, unstagedAvailability, IsRightSideInUse);
            menu.UpdateMenu(isWindowToSmall, ConsoleKey.NoName);
            WasWindowResizedSmaller = new MenuWriter(menu.LastPageDisplayed, isWindowToSmall, consoleWrapper).UpdateDisplay(WasWindowResizedSmaller);
            var windowSizeTemp = windowSize;
            var keys = new[] { ConsoleKey.A, ConsoleKey.Enter, ConsoleKey.S };
            do
            {
                key = consoleWrapper.ReadKey(true).Key;
                unstagedAvailability = menu.UnstagedAvailability;

                if (key.Equals(ConsoleKey.RightArrow))
                {
                    unstaged = menu.Unstaged;
                    staged = menu.Staged;
                    DisplayHighlightedHunksPage(ref unstaged, ref staged, unstagedAvailability);
                    menu.Unstaged = unstaged;
                    menu.Staged = staged;
                }

                menu.WasStagingKeyPressed = keys.Any(necessaryKey => necessaryKey.Equals(key));
                if (menu.WasStagingKeyPressed)
                {
                    ApplyStageOrUnstageOption(ref menu, ref unstaged, ref staged, unstagedAvailability);
                }
                else if (key.Equals(ConsoleKey.C))
                {
                    DisplayCommitPage(menu, staged);
                }
                else if (key.Equals(ConsoleKey.B))
                {
                    DisplayBranchesMenu(menu, isWindowToSmall);
                }
                else if (key.Equals(ConsoleKey.Escape) || menuKeys.Contains(key))
                {
                    break;
                }

                menu.UpdateMenu(isWindowToSmall, key);

                if (!menu.WasInteractorModified && !menu.WasStagingKeyPressed && key != ConsoleKey.Escape)
                {
                    continue;
                }

                windowWidth = consoleWrapper.WindowWidth;
                windowSize = consoleWrapper.WindowHeight * windowWidth;
                WasWindowResizedSmaller = windowSize < windowSizeTemp;
                windowSizeTemp = windowSize;
                isWindowToSmall = windowHeight < 8 && windowWidth < 35;
                WasWindowResizedSmaller = new MenuWriter(menu.LastPageDisplayed, isWindowToSmall, consoleWrapper).UpdateDisplay(WasWindowResizedSmaller);
            }
            while (true);
        }

        private void ApplyStageOrUnstageOption(ref StatusMenu menu, ref ModifiedFiles unstaged, ref ModifiedFiles staged, bool unstagedAvailability)
        {
            unstaged = menu.Unstaged;
            staged = menu.Staged;

            if (unstagedAvailability)
            {
                var current = unstaged[unstaged.Current].GetElement();
                if (key.Equals(ConsoleKey.Enter) && current != "")
                {
                    gitManager.StageFile(current);
                    menu.Unstaged.Current = Math.Max(0, menu.Unstaged.Current - 1);
                    menu.Unstaged.Index = Math.Min(menu.Unstaged.Current, menu.Unstaged.Index);
                }
                else if (key.Equals(ConsoleKey.A))
                {
                    gitManager.StageAllFiles();
                    menu.Unstaged.Current = 0;
                    menu.Unstaged.Index = 0;
                }
            }
            else
            {
                var current = staged[staged.Current].GetElement();
                if (key.Equals(ConsoleKey.Enter) && current != "")
                {
                    gitManager.UnstageFile(current);
                    menu.Staged.Current = Math.Max(0, menu.Staged.Current - 1);
                    menu.Staged.Index = Math.Min(menu.Staged.Current, menu.Staged.Index);
                }
                else if (key.Equals(ConsoleKey.A))
                {
                    gitManager.UnstageAllFiles();
                    menu.Staged.Current = 0;
                    menu.Staged.Index = 0;
                }
            }

            unstaged = new ModifiedFiles(gitManager.GetUnstagedFiles());
            unstaged.Index = menu.Unstaged.Index;
            unstaged.Current = menu.Unstaged.Current;
            staged = new ModifiedFiles(gitManager.GetStagedFiles());
            staged.Index = menu.Staged.Index;
            staged.Current = menu.Staged.Current;
            menu.Unstaged = unstaged;
            menu.Staged = staged;
        }

        private (int index, int current) SetModifiedFileIndeexes(ModifiedFiles modifiedFilesToSet, ModifiedFiles modifiedFilesToUse, int height)
        {
            if (modifiedFilesToUse.Current == 0 && key == ConsoleKey.UpArrow)
            {
                modifiedFilesToSet.Current = modifiedFilesToSet.ListCount - 1;
                modifiedFilesToSet.Index = modifiedFilesToSet.ListCount - height < 0 ? 0 : modifiedFilesToSet.ListCount - height;
            }
            else
            {
                modifiedFilesToSet.Current = 0;
                modifiedFilesToSet.Index = 0;
            }

            return (modifiedFilesToSet.Index, modifiedFilesToSet.Current);
        }

        private void DisplayHighlightedHunksPage(ref ModifiedFiles unstaged, ref ModifiedFiles staged, bool unstagedAvailability)
        {
            var currentElement = unstagedAvailability ? unstaged[unstaged.Current].GetElement() : staged[staged.Current].GetElement();
            if (currentElement == "")
            {
                return;
            }

            var windowWidth = consoleWrapper.WindowWidth;
            var windowHeight = consoleWrapper.WindowHeight;
            var windowSize = windowHeight * windowWidth;
            var isWindowToSmall = windowHeight < 8 && windowWidth < 35;
            var diff = new Diff(unstagedAvailability
                ? gitManager.GetUnstagedDiff(
                    currentElement,
                    gitManager.GetRepoPath(),
                    unstaged.GetText().Count >= 1)
                : gitManager.GetStagedDiff(staged[staged.Current].GetElement(), gitManager.GetRepoPath()));
            var menu = new HighlightedHunksMenu(consoleWrapper, gitManager, diff, unstaged, staged, unstagedAvailability, IsRightSideInUse);
            menu.UpdateMenu(isWindowToSmall, key);
            WasWindowResizedSmaller = new MenuWriter(menu.LastPageDisplayed, isWindowToSmall, consoleWrapper).UpdateDisplay(WasWindowResizedSmaller);
            var windowSizeTemp = windowSize;

            do
            {
                key = consoleWrapper.ReadKey(true).Key;
                var stageSuccesful = SelectStageOptionForDiffMenu(ref menu, ref diff, ref unstaged, ref staged, unstagedAvailability);
                menu.UpdateMenu(isWindowToSmall, key);

                if (GetExitConditionForHighlightedHunksPage(unstagedAvailability ? unstaged[unstaged.Current] : staged[staged.Current], diff[diff.Current]))
                {
                    break;
                }
                else if (key.Equals(ConsoleKey.B))
                {
                    DisplayBranchesMenu(menu, isWindowToSmall);
                }
                else if (key.Equals(ConsoleKey.C))
                {
                    DisplayCommitPage(menu, staged);
                }
                else if (!menu.WasInteractorModified && !stageSuccesful)
                {
                    continue;
                }

                windowWidth = consoleWrapper.WindowWidth;
                windowSize = consoleWrapper.WindowHeight * windowWidth;
                WasWindowResizedSmaller = windowSize < windowSizeTemp;
                windowSizeTemp = windowSize;
                isWindowToSmall = windowHeight < 8 && windowWidth < 35;
                WasWindowResizedSmaller = new MenuWriter(menu.LastPageDisplayed, isWindowToSmall, consoleWrapper).UpdateDisplay(WasWindowResizedSmaller);
            }
            while (true);
        }

        private bool SelectStageOptionForDiffMenu(
            ref HighlightedHunksMenu menu,
            ref Diff diff,
            ref ModifiedFiles unstaged,
            ref ModifiedFiles staged,
            bool unstagedAvailability)
        {
            var stageSuccesful = false;
            diff = menu.Diff;
            unstaged = menu.Unstaged;
            staged = menu.Staged;
            var currentFile = unstagedAvailability
                ? unstaged[unstaged.Current].GetElement()
                : staged[staged.Current].GetElement();
            var repoPath = gitManager.GetRepoPath();
            if (key.Equals(ConsoleKey.Enter))
            {
                UpdateVariablesWhenStagingOrUnstagingHunk(ref diff, ref unstaged, ref staged, repoPath, unstagedAvailability);
                stageSuccesful = true;
            }
            else if (key.Equals(ConsoleKey.S))
            {
                UpdateVariablesWhenStagingOrUnstagingLine(ref diff, unstagedAvailability, ref unstaged, ref staged);
                stageSuccesful = true;
            }
            else if (key.Equals(ConsoleKey.A))
            {
                if (unstagedAvailability)
                {
                    gitManager.StageFile(currentFile);
                    stageSuccesful = true;
                }
                else
                {
                    gitManager.UnstageFile(currentFile);
                    stageSuccesful = true;
                }

                unstaged = new ModifiedFiles(gitManager.GetUnstagedFiles());
                staged = new ModifiedFiles(gitManager.GetStagedFiles());
            }

            menu.Diff = diff;
            menu.Unstaged = unstaged;
            menu.Staged = staged;

            return stageSuccesful;
        }

        private void UpdateVariablesWhenStagingOrUnstagingHunk(
            ref Diff diff,
            ref ModifiedFiles unstaged,
            ref ModifiedFiles staged,
            string repoPath,
            bool unstagedAvailability)
        {
            var currentUnstaged = unstaged[unstaged.Current].GetElement();
            var currentStaged = staged[staged.Current].GetElement();
            var diffIndex = diff.Index == 0 ? 0 : diff.Index;
            var diffCurrent = diff.Current == 0 ? 0 : diff.Current;
            var currentFile = unstagedAvailability ? currentUnstaged : currentStaged;
            var patchPath = new Patcher(repoPath, gitManager, diff, currentFile != "").GetHunkPatchPath();
            var unstagedIndex = unstaged.Index;
            var unstagedCurrent = unstaged.Current;
            unstaged = new ModifiedFiles(gitManager.GetUnstagedFiles());
            unstaged.Current = unstagedCurrent;
            unstaged.Index = unstagedIndex;
            var stagedIndex = staged.Index;
            var stagedCurrent = staged.Current;
            staged = new ModifiedFiles(gitManager.GetStagedFiles());
            staged.Current = stagedCurrent;
            staged.Index = stagedIndex;

            if (unstagedAvailability)
            {
                gitManager.Stage(patchPath);
            }
            else
            {
                var tempFilePath = Path.GetTempPath() + "tempFile" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".txt";
                System.IO.File.Create(tempFilePath).Close();
                var currentElementLines = System.IO.File.ReadAllLines(repoPath + "/" + currentFile);
                currentElementLines[0] = "﻿\ufeff" + currentElementLines[0];
                System.IO.File.WriteAllLines(tempFilePath, currentElementLines);
                gitManager.RestoreFileToMatchIndex(currentFile);
                gitManager.UnstageFile(currentFile);
                gitManager.ApplyPatchInReverse(patchPath);
                gitManager.StageFile(currentFile);
                System.IO.File.WriteAllLines(repoPath + "/" + currentFile, System.IO.File.ReadAllLines(tempFilePath));
                System.IO.File.Delete(tempFilePath);
            }

            if (System.IO.File.Exists(patchPath))
            {
                System.IO.File.Delete(patchPath);
            }

            diff = unstagedAvailability
                ? new Diff(gitManager.GetUnstagedDiff(currentUnstaged, repoPath, currentFile != "" && unstaged.GetText().Count >= 1))
                : new Diff(gitManager.GetStagedDiff(currentStaged, repoPath));
            diff.Current = Math.Min(diffCurrent, diff.GetText().Count - 1);
            diff.Index = diffIndex;
        }

        private void UpdateVariablesWhenStagingOrUnstagingLine(
            ref Diff diff,
            bool unstagedAvailability,
            ref ModifiedFiles unstaged,
            ref ModifiedFiles staged)
        {
            var diffElements = diff.GetText();
            var currentStaged = staged[staged.Current].GetElement();
            var currentUnstaged = unstaged[unstaged.Current].GetElement();
            var repoPath = gitManager.GetRepoPath();
            var currentFile = unstagedAvailability ? currentUnstaged : currentStaged;
            var diffIndex = diff.Index == 0 ? 0 : diff.Index;
            var diffCurrent = diff.Current == 0 ? 0 : diff.Current - 1;
            var previous = diff[Math.Max(0, diff.Current - 1)].GetElement();
            var next = diff[Math.Min(diffElements.Count - 1, diff.Current + 1)].GetElement();
            var isLineReplaced = previous.StartsWith("-") && diff[diff.Current].GetElement().StartsWith("+");
            isLineReplaced = isLineReplaced || (diff[diff.Current].GetElement().StartsWith("-") && next.StartsWith("+"));
            var patchPath = new Patcher(gitManager.GetRepoPath(), gitManager, diff, currentFile != "")
                .GetLinePatchPath(
                    isLineReplaced,
                    staged.GetText().Any(unstagedFile => unstagedFile.GetElement() == currentUnstaged),
                    currentFile);

            if (unstagedAvailability)
            {
                gitManager.Stage(patchPath);
            }
            else
            {
                var tempFilePath = Path.GetTempPath() + "tempFile" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".txt";
                string[] currentElementLines;
                System.IO.File.Create(tempFilePath).Close();
                currentElementLines = System.IO.File.ReadAllLines(repoPath + "/" + currentFile);
                currentElementLines[0] = "﻿\ufeff" + currentElementLines[0];
                System.IO.File.WriteAllLines(tempFilePath, currentElementLines);
                gitManager.RestoreFileToMatchIndex(currentFile);
                gitManager.UnstageFile(currentFile);
                gitManager.ApplyPatchInReverse(patchPath);
                gitManager.StageFile(currentFile);
                System.IO.File.WriteAllLines(repoPath + "/" + currentFile, System.IO.File.ReadAllLines(tempFilePath));
                System.IO.File.Delete(tempFilePath);
            }

            if (System.IO.File.Exists(patchPath))
            {
                System.IO.File.Delete(patchPath);
            }

            var unstagedIndex = unstaged.Index;
            var unstagedCurrent = unstaged.Current;
            unstaged = new ModifiedFiles(gitManager.GetUnstagedFiles());
            unstaged.Current = unstagedCurrent;
            unstaged.Index = unstagedIndex;
            var stagedIndex = staged.Index;
            var stagedCurrent = staged.Current;
            staged = new ModifiedFiles(gitManager.GetStagedFiles());
            staged.Index = stagedIndex;
            staged.Current = stagedCurrent;
            diff = unstagedAvailability
                ? new Diff(gitManager.GetUnstagedDiff(currentUnstaged, repoPath, currentFile != "" && unstaged.GetText().Count >= 1))
                : new Diff(gitManager.GetStagedDiff(currentStaged, repoPath));
            diff.Current = Math.Min(diffCurrent, diff.GetText().Count - 1);
            diff.Index = diffIndex;
        }

        private List<string> SelectSide(List<string> leftSide, List<string> rightSide, KeyInteractor<IElement> keyInteractor)
        {
            IsRightSideInUse = keyInteractor.KeyInfo.Equals(ConsoleKey.Spacebar) ? !IsRightSideInUse : IsRightSideInUse;
            return IsRightSideInUse ? rightSide : leftSide;
        }

        private List<string> SelectScreen(List<string> leftSide, List<string> rightSide, KeyInteractor<IElement> keyInteractor)
        {
            var height = consoleWrapper.WindowHeight;
            var width = consoleWrapper.WindowWidth;
            if (height < 15 && width < 50)
            {
                return SelectSide(leftSide, rightSide, keyInteractor);
            }

            return MergeSides(leftSide, rightSide);
        }

        private void UpdateWriter(List<string> toWrite, bool isWindowToSmall, List<string>? overlayPage = null)
        {
            if (isWindowToSmall)
            {
                writer = new StringWriter();
                consoleWrapper.Clear();
                consoleWrapper.Write("Increase window size!");
            }
            else
            {
                var paddedPage = new PageUtilities(toWrite.ToArray()).PadElement(consoleWrapper.WindowWidth);
                if (WasWindowResizedSmaller)
                {
                    WasWindowResizedSmaller = false;
                    consoleWrapper.Clear();
                }

                consoleWrapper.SetCursorPosition(0, 0);
                Write(paddedPage);

                if (overlayPage != null)
                {
                    var width = consoleWrapper.WindowWidth;
                    var height = consoleWrapper.WindowHeight;
                    var left = (int)(0.125 * width);
                    var top = (int)(0.125 * height);
                    WriteOverlay(overlayPage, ref left, ref top);
                }
            }
        }

        private void WriteOverlay(List<string> overlayPage, ref int left, ref int top)
        {
            var width = consoleWrapper.WindowWidth;
            var height = consoleWrapper.WindowHeight;

            if (height < 15 && width < 60)
            {
                left = 0;
                top = 0;
            }

            foreach (var line in overlayPage)
            {
                Console.SetCursorPosition(left, top);
                consoleWrapper.Write(line);
                top++;
            }
        }

        private void Write(List<string> paddedPage)
        {
            writer = new StringWriter();
            paddedPage.ForEach(element => writer.WriteLine(element));
            var result = writer.ToString();
            consoleWrapper.Write(result == null ? "" : result[..^1].TrimStart());
        }

        private void UpdateLeftSideVariables(
            ref List<IElement> elements,
            ref List<string> leftSide,
            bool isWindowToSmall,
            IInteractor<IElement> interactor,
            int width)
        {
            var windowWidth = consoleWrapper.WindowWidth;
            var windowHeight = consoleWrapper.WindowHeight;
            if (isWindowToSmall)
            {
                return;
            }

            if (windowWidth < 50 && windowHeight < 15)
            {
                elements = interactor.GetPageDataAndSize(width * (windowHeight - 4), width - 2).list;
                leftSide = interactor.GetHighlightedSide(windowHeight - 4, width, width * (windowHeight - 4), true);
            }
            else
            {
                elements = interactor.GetPageDataAndSize(width * (windowHeight - 4), width, true).list;
                leftSide = interactor.GetHighlightedSide(windowHeight - 4, width, width * (windowHeight - 4), true);
            }
        }

        private void DisplayCommitDetailsMenu()
        {
            var windowWidth = consoleWrapper.WindowWidth;
            var windowHeight = consoleWrapper.WindowHeight;
            var windowSize = windowHeight * windowWidth;
            var menu = new CommitDetailsMenu(consoleWrapper, gitManager, commits, IsRightSideInUse);
            var isWindowToSmall = consoleWrapper.WindowHeight < 11 && consoleWrapper.WindowWidth < 20;
            var windowSizeTemp = windowSize;
            menu.UpdateMenu(isWindowToSmall, key);
            WasWindowResizedSmaller = new MenuWriter(menu.LastPageDisplayed, isWindowToSmall, consoleWrapper).UpdateDisplay(WasWindowResizedSmaller);
            do
            {
                key = consoleWrapper.ReadKey(true).Key;
                menu.UpdateMenu(isWindowToSmall, key);
                if (key.Equals(ConsoleKey.Escape) || menuKeys.Contains(key))
                {
                    break;
                }
                else if (key == ConsoleKey.RightArrow)
                {
                    DisplayDiffMenu(commits[commits.Current], commits[commits.Current + 1]);
                }
                else if (key == ConsoleKey.B)
                {
                    DisplayBranchesMenu(menu, isWindowToSmall);
                }
                else if (!menu.WasInteractorModified)
                {
                    continue;
                }

                windowHeight = consoleWrapper.WindowHeight;
                windowSize = windowHeight * consoleWrapper.WindowWidth;
                WasWindowResizedSmaller = windowSize < windowSizeTemp;
                windowSizeTemp = windowSize;
                WasWindowResizedSmaller = new MenuWriter(menu.LastPageDisplayed, isWindowToSmall, consoleWrapper).UpdateDisplay(WasWindowResizedSmaller);
            }
            while (true);
        }

        private void DisplayDiffMenu(IElement currentCommit, IElement previousCommit)
        {
            var windowWidth = consoleWrapper.WindowWidth;
            var windowHeight = consoleWrapper.WindowHeight;
            var windowSize = windowHeight * windowWidth;
            var modifiedFiles = new ModifiedFiles(gitManager.GetCommitModifiedFileNames(currentCommit.Id));
            var diff = new Diff(gitManager.GetDiffBetweenConsecutiveCommits(previousCommit.Id, currentCommit.Id, gitManager.GetRepoPath(), modifiedFiles[0].GetElement()));
            var menu = new DiffMenu(consoleWrapper, modifiedFiles, diff, previousCommit, currentCommit, gitManager, IsRightSideInUse);
            var isWindowToSmall = consoleWrapper.WindowHeight < 11 && consoleWrapper.WindowWidth < 20;
            var windowSizeTemp = windowSize;
            menu.UpdateMenu(isWindowToSmall, key);
            WasWindowResizedSmaller = new MenuWriter(menu.LastPageDisplayed, isWindowToSmall, consoleWrapper).UpdateDisplay(WasWindowResizedSmaller);

            do
            {
                key = consoleWrapper.ReadKey(true).Key;
                menu.UpdateMenu(isWindowToSmall, key);
                if (key.Equals(ConsoleKey.RightArrow))
                {
                    DisplayHighlightedDiffMenu(diff, modifiedFiles);
                }
                else if (key.Equals(ConsoleKey.B))
                {
                    DisplayBranchesMenu(menu, isWindowToSmall);
                }
                else if (key.Equals(ConsoleKey.Escape) || key.Equals(ConsoleKey.LeftArrow) || menuKeys.Contains(key))
                {
                    break;
                }
                else if (!menu.WasInteractorModified)
                {
                    continue;
                }

                windowHeight = consoleWrapper.WindowHeight;
                windowSize = windowHeight * consoleWrapper.WindowWidth;
                WasWindowResizedSmaller = windowSize < windowSizeTemp;
                windowSizeTemp = windowSize;
                WasWindowResizedSmaller = new MenuWriter(menu.LastPageDisplayed, isWindowToSmall, consoleWrapper).UpdateDisplay(WasWindowResizedSmaller);
            }
            while (true);
        }

        private void DisplayHighlightedDiffMenu(Diff diff, ModifiedFiles modifiedFiles)
        {
            var windowWidth = consoleWrapper.WindowWidth;
            var windowHeight = consoleWrapper.WindowHeight;
            var windowSize = windowHeight * windowWidth;
            var menu = new HighlightedDiffMenu(consoleWrapper, diff, modifiedFiles, IsRightSideInUse);
            var isWindowToSmall = consoleWrapper.WindowHeight < 11 && consoleWrapper.WindowWidth < 20;
            var windowSizeTemp = windowSize;
            menu.UpdateMenu(isWindowToSmall, key);
            WasWindowResizedSmaller = new MenuWriter(menu.LastPageDisplayed, isWindowToSmall, consoleWrapper).UpdateDisplay(WasWindowResizedSmaller);

            do
            {
                key = consoleWrapper.ReadKey(true).Key;
                menu.UpdateMenu(isWindowToSmall, key);
                if (key.Equals(ConsoleKey.B))
                {
                    DisplayBranchesMenu(menu, isWindowToSmall);
                }
                else if (key.Equals(ConsoleKey.Escape) || key.Equals(ConsoleKey.LeftArrow) || menuKeys.Contains(key))
                {
                    break;
                }

                windowHeight = consoleWrapper.WindowHeight;
                windowSize = windowHeight * consoleWrapper.WindowWidth;
                WasWindowResizedSmaller = windowSize < windowSizeTemp;
                windowSizeTemp = windowSize;
                WasWindowResizedSmaller = new MenuWriter(menu.LastPageDisplayed, isWindowToSmall, consoleWrapper).UpdateDisplay(WasWindowResizedSmaller);
            }
            while (true);
        }

        private int SetTextWidth(bool forLeftSide)
        {
            var windowWidth = consoleWrapper.WindowWidth;
            var result = forLeftSide ? (int)(0.6 * windowWidth) - 2 : windowWidth - (int)(0.6 * windowWidth) - 2;
            var windowHeight = consoleWrapper.WindowHeight;
            return windowWidth < 50 && windowHeight < 15
                ? windowWidth - 2
                : result;
        }

        private List<string> UpdateDiffPage(
            List<string> leftSide,
            List<string> diff,
            int rowSize,
            KeyInteractor<IElement> keyInteractor,
            bool shouldColorBoxLines = false)
        {
            var windowHeight = consoleWrapper.WindowHeight;
            var windowWidth = consoleWrapper.WindowWidth;
            List<string> rightSide;
            if (windowHeight < 15 && windowWidth < 50)
            {
                rightSide = new BoxDrawer(diff, "Diff").DrawBox(windowWidth, windowHeight - 2, false, shouldColorBoxLines);
                leftSide = new BoxDrawer(leftSide, "Files").DrawBox(windowWidth, windowHeight - 2);
            }
            else
            {
                rightSide = new BoxDrawer(diff, "Diff").DrawBox(rowSize, windowHeight - 2, false, shouldColorBoxLines);
                leftSide = new BoxDrawer(leftSide, "Files").DrawBox((int)(0.6 * windowWidth), windowHeight - 2);
            }

            return SelectScreen(leftSide, rightSide, keyInteractor);
        }

        private List<string> UpdateHighlightedHunksPage(
            List<string> diff,
            ModifiedFiles unstaged,
            ModifiedFiles staged,
            KeyInteractor<IElement> keyInteractor,
            bool unstagedAvailability)
        {
            var windowHeight = consoleWrapper.WindowHeight;
            var windowWidth = consoleWrapper.WindowWidth;
            var unstagedHeight = (windowHeight - 2) / 2 + (windowHeight - 2) % 2 - 2;
            var unstagedAvailableSpace = ((int)(0.6 * windowWidth) - 2) * unstagedHeight;
            List<string> rightSide;
            List<string> leftSide;
            var branch = gitManager.GetCurrentGitBranch();
            var noOfCommitsToPull = gitManager.GetNumberOfCommitsToPull(branch);
            var noOfCommitsToPush = gitManager.GetNumberOfCommitsToPush(branch);
            var shouldDisplayNoOfCommits = noOfCommitsToPull != "" && noOfCommitsToPush != "";
            if (windowHeight < 15 && windowWidth < 50)
            {
                rightSide = new BoxDrawer(diff, "Diff").DrawBox(windowWidth, windowHeight - 2, false, true);

                var unstagedToString = unstaged.GetHighlightedSide(unstagedHeight, windowWidth - 2, (unstagedHeight - 3) * (windowWidth - 2));
                var stagedToString = staged.GetHighlightedSide(
                        windowHeight - ((windowHeight - 2) / 2 + (windowHeight - 2) % 2 - 2) - 2,
                        (int)(0.6 * windowWidth) - 2,
                        ((windowHeight - 2) / 2 - 2) * (int)(0.6 * windowWidth) - 2);
                if (unstagedAvailability)
                {
                    stagedToString[0] = stagedToString[0].Replace("\u001b[7m", "");
                    stagedToString[^1] = stagedToString[^1].Replace("\u001b[7m", "");
                }
                else
                {
                    unstagedToString[0] = unstagedToString[0].Replace("\u001b[7m", "");
                    unstagedToString[^1] = unstagedToString[^1].Replace("\u001b[7m", "");
                }

                leftSide = new BoxDrawer(unstagedToString, "Unstaged").DrawBox(
                            (int)(0.6 * windowWidth),
                            unstagedHeight + 2,
                            false,
                            false,
                            shouldDisplayNoOfCommits ? $"↑{noOfCommitsToPush} ↓{noOfCommitsToPull}" + " {" + branch + "}" : "{" + branch + "}");
                stagedToString = new BoxDrawer(stagedToString, "Staged").DrawBox(windowWidth, windowHeight - unstagedHeight - 4, true);
                leftSide.AddRange(stagedToString);
            }
            else
            {
                var textwidth = new TextWidthSetter(consoleWrapper);
                var leftSideWidth = textwidth.SetTextWidth(true);
                rightSide = new BoxDrawer(diff, "Diff").DrawBox(textwidth.SetTextWidth(false) + 2, windowHeight - 2, false, true);

                var unstagedToString = unstaged.GetHighlightedSide(unstagedHeight, leftSideWidth, unstagedAvailableSpace);
                var stagedToString = staged.GetHighlightedSide(
                        windowHeight - ((windowHeight - 2) / 2 + (windowHeight - 2) % 2 - 2) - 2,
                        (int)(0.6 * windowWidth) - 2,
                        ((windowHeight - 2) / 2 - 2) * (int)(0.6 * windowWidth) - 2);
                if (unstagedAvailability)
                {
                    stagedToString[0] = stagedToString[0].Replace("\u001b[7m", "");
                    stagedToString[^1] = stagedToString[^1].Replace("\u001b[7m", "");
                }
                else
                {
                    unstagedToString[0] = unstagedToString[0].Replace("\u001b[7m", "");
                    unstagedToString[^1] = unstagedToString[^1].Replace("\u001b[7m", "");
                }

                leftSide = new BoxDrawer(unstagedToString, "Unstaged").DrawBox(
                            (int)(0.6 * windowWidth),
                            unstagedHeight + 2,
                            false,
                            false,
                            shouldDisplayNoOfCommits ? $"↑{noOfCommitsToPush} ↓{noOfCommitsToPull}" + " {" + branch + "}" : "{" + branch + "}");
                stagedToString = new BoxDrawer(stagedToString, "Staged").DrawBox((int)(0.6 * windowWidth), windowHeight - unstagedHeight - 4, true);
                leftSide.AddRange(stagedToString);
            }

            return SelectScreen(leftSide, rightSide, keyInteractor);
        }

        private List<string> UpdateExtendedPage(List<string> leftSideCommits, KeyInteractor<IElement> keyInteractor)
        {
            var width = consoleWrapper.WindowWidth;
            var height = consoleWrapper.WindowHeight;
            var rightSide = consoleWrapper.WindowHeight < 15 && consoleWrapper.WindowWidth < 50
                ? commits.FormateUnhighlightedSide(height - 2, width, gitManager)
                : commits.FormateUnhighlightedSide(height - 2, width - (int)(0.6 * width), gitManager);

            return SelectScreen(
                new BoxDrawer(leftSideCommits, "Commits").DrawBox(height < 15 && width < 50 ? width : (int)(0.6 * width), height - 2),
                rightSide,
                keyInteractor);
        }

        private List<string> UpdatePage(ref int commitOcupancyRate, ref List<IElement> page)
        {
            var keyInteractor = new KeyInteractor<IElement>(commits)
            {
                KeyInfo = key
            };

            var windowWidth = consoleWrapper.WindowWidth;
            var windowHeight = consoleWrapper.WindowHeight;
            var windowSize = windowHeight * windowWidth;
            var commitsAvailableSpace = windowSize - 4 * windowWidth;
            var availableSpace = commitsAvailableSpace - commitsAvailableSpace / windowWidth * 2;
            var isPageLargerThanWindow = commitOcupancyRate > availableSpace;
            var commitsToString = new List<string>();
            page.ForEach(line =>
            {
                var commitDetails = line.GetElement().Split("\n");
                commitsToString.Add(commitDetails[0] + " " + commitDetails[1] + " " + commitDetails[2] + " " + commitDetails[3]);
            });
            (commits.Index, commits.Current) = page.Count == 0
                ? (0, 0)
                : keyInteractor.KeySwitch(commitsToString, page[^1].Id, page[0].Id, isPageLargerThanWindow, windowWidth, commits[commits.Current].GetElement().Length - 40);
            (page, commitOcupancyRate) = commits.GetPageDataAndSize(availableSpace, windowWidth);
            var toWrite = commits.GetHighlightedSide(windowHeight - 4, windowWidth - 2, availableSpace);
            return new BoxDrawer(toWrite, "Commits").DrawBox(windowWidth, windowHeight - 2, true);
        }

        private List<string> UpdateStatusPage(
            ref List<IElement> unstagedElements,
            ref List<IElement> stagedElements,
            ref List<string> unstagedPageList,
            ref List<string> stagedPageList,
            ref ModifiedFiles unstaged,
            ref ModifiedFiles staged,
            bool isUnstagedInUse)
        {
            var windowHeight = consoleWrapper.WindowHeight;
            var windowWidth = consoleWrapper.WindowWidth;
            var unstagedHeight = (windowHeight - 2) / 2 + (windowHeight - 2) % 2 - 2;
            var stagedHeight = windowHeight - unstagedHeight;
            var unstagedToString = new List<string>();
            var stagedToString = new List<string>();
            var tempList = new List<string>();
            var branch = gitManager.GetCurrentGitBranch();
            var noOfCommitsToPull = gitManager.GetNumberOfCommitsToPull(branch);
            var noOfCommitsToPush = gitManager.GetNumberOfCommitsToPush(branch);
            var shouldDisplayNoOfCommits = noOfCommitsToPush != "" && noOfCommitsToPull != "";
            Diff diff;

            if (isUnstagedInUse)
            {
                var keyInteractor = new KeyInteractor<IElement>(unstaged);
                keyInteractor.KeyInfo = key;
                (unstaged.Index, unstaged.Current) = keyInteractor
                    .KeySwitch(unstagedPageList, unstagedElements[^1].Id, unstagedElements[0].Id, false, (int)(0.6 * windowWidth) - 2, unstaged[unstaged.Current].GetElement().Length);
                stagedPageList = staged.GetHighlightedSide(stagedHeight, (int)(0.6 * windowWidth) - 2, stagedHeight * (int)(0.6 * windowWidth) - 2);
                stagedToString.AddRange(stagedPageList);
                stagedToString[0] = stagedToString[0].Replace("\x1b[7m", "");
                stagedToString[^1] = stagedToString[^1].Replace("\x1b[7m", "");
                var unstagedAvailableSpace = ((int)(0.6 * windowWidth) - 2) * unstagedHeight;
                unstagedElements = unstaged.GetPageDataAndSize(unstagedAvailableSpace, (int)(0.6 * windowWidth) - 2).list;
                unstagedPageList = unstaged[unstaged.Current].GetElement() == ""
                       ? new List<string>()
                       : unstaged.GetHighlightedSide(unstagedHeight, (int)(0.6 * windowWidth) - 2, unstagedAvailableSpace);
                var currentUnstaged = unstaged[unstaged.Current].GetElement();
                diff = new Diff(gitManager.GetUnstagedDiff(currentUnstaged, gitManager.GetRepoPath(), currentUnstaged != "" && unstaged.GetText().Count >= 1));
                tempList.AddRange(new BoxDrawer(unstagedPageList, "Unstaged").DrawBox(
                            (int)(0.6 * windowWidth),
                            unstagedHeight + 2,
                            false,
                            false,
                            shouldDisplayNoOfCommits ? $"↑{noOfCommitsToPush} ↓{noOfCommitsToPull}" + " {" + branch + "}" : "{" + branch + "}"));
                stagedToString = new BoxDrawer(stagedToString, "Staged").DrawBox((int)(0.6 * windowWidth), (windowHeight - 2) / 2, true);
                tempList.AddRange(stagedToString);
            }
            else
            {
                var keyInteractor = new KeyInteractor<IElement>(staged);
                keyInteractor.KeyInfo = key;
                (staged.Index, staged.Current) = keyInteractor
                    .KeySwitch(stagedPageList, stagedElements[^1].Id, stagedElements[0].Id, false, (int)(0.6 * windowWidth) - 2, staged[staged.Current].GetElement().Length);
                unstagedPageList = unstaged.GetHighlightedSide(unstagedHeight, (int)(0.6 * windowWidth) - 2, unstagedHeight * (int)(0.6 * windowWidth) - 2);
                unstagedToString.AddRange(unstagedPageList);
                unstagedToString[0] = unstagedToString[0].Replace("\x1b[7m", "");
                unstagedToString[^1] = unstagedToString[^1].Replace("\x1b[7m", "");
                var stagedAvailableSpace = ((int)(0.6 * windowWidth) - 2) * stagedHeight;
                stagedElements = staged.GetPageDataAndSize(stagedAvailableSpace, (int)(0.6 * windowWidth) - 2).list;
                stagedPageList = staged[staged.Current].GetElement() == ""
                       ? new List<string>()
                       : staged.GetHighlightedSide(stagedHeight, (int)(0.6 * windowWidth) - 2, stagedAvailableSpace);
                var currentStaged = staged[staged.Current].GetElement();
                diff = new Diff(gitManager.GetStagedDiff(currentStaged, gitManager.GetRepoPath()));
                tempList.AddRange(new BoxDrawer(unstagedToString, "Unstaged").DrawBox(
                            (int)(0.6 * windowWidth),
                            unstagedHeight + 2,
                            false,
                            false,
                            shouldDisplayNoOfCommits ? $"↑{noOfCommitsToPush} ↓{noOfCommitsToPull}" + " {" + branch + "}" : "{" + branch + "}"));
                stagedToString = new BoxDrawer(stagedPageList, "Staged").DrawBox((int)(0.6 * windowWidth), (windowHeight - 2) / 2, true);
                tempList.AddRange(stagedToString);
            }

            var diffAvailableSpace = (windowWidth - (int)(0.6 * windowWidth) - 2) * (windowHeight - 4);
            var rightSide = new BoxDrawer(
                    diff[0].GetElement() == ""
                        ? new List<string>()
                        : diff.GetHighlightedSide(windowHeight - 4, windowWidth - (int)(0.6 * windowWidth) - 2, diffAvailableSpace, false), "Diff")
                            .DrawBox(windowWidth - (int)(0.6 * windowWidth), windowHeight - 2);
            var result = MergeSides(tempList, rightSide);
            var additionalMenuBar = SetMenuBarForStatus(isUnstagedInUse);
            result.Insert(0, MenuBar);
            result.Add(additionalMenuBar);
            return result;
        }

        private void DisplayBranchesMenu(Menu background, bool isWindowToSmall)
        {
            var branchesNames = gitManager.GetBranchesNames();
            if (branchesNames == "")
            {
                return;
            }

            var height = consoleWrapper.WindowHeight;
            var width = consoleWrapper.WindowWidth;
            var windowSize = height * width;
            var branches = new ModifiedFiles(branchesNames);
            var keyInteractor = new KeyInteractor<IElement>(branches);
            keyInteractor.KeyInfo = ConsoleKey.NoName;
            var menu = new BranchMenu(consoleWrapper, gitManager, branchesNames, branches);
            menu.UpdateMenu(isWindowToSmall, key);
            WasWindowResizedSmaller = new MenuWriter(background.LastPageDisplayed, isWindowToSmall, consoleWrapper)
                .UpdateDisplay(WasWindowResizedSmaller, menu.LastPageDisplayed);

            do
            {
                var windowSizeTemp = windowSize;
                key = consoleWrapper.ReadKey(true).Key;
                menu.UpdateMenu(isWindowToSmall, key);
                height = consoleWrapper.WindowHeight;
                width = consoleWrapper.WindowWidth;
                windowSize = height * width;
                keyInteractor.KeyInfo = key;
                if (keyInteractor.KeyInfo == ConsoleKey.Escape)
                {
                    break;
                }
                else if (keyInteractor.KeyInfo == ConsoleKey.Enter)
                {
                    var currentBranchName = branches[branches.Current].GetElement();
                    gitManager.ChangeBranch(currentBranchName.StartsWith("* ") ? currentBranchName[2..] : currentBranchName.Trim());
                    branchesNames = gitManager.GetBranchesNames();
                    branches = new ModifiedFiles(branchesNames);
                    menu.Branches = branches;
                    menu.BranchesNames = branchesNames;
                }

                WasWindowResizedSmaller = windowSizeTemp > windowSize;
                if (WasWindowResizedSmaller)
                {
                    background.UpdateMenu(isWindowToSmall, key);
                }

                WasWindowResizedSmaller = new MenuWriter(background.LastPageDisplayed, isWindowToSmall, consoleWrapper)
                    .UpdateDisplay(WasWindowResizedSmaller, menu.LastPageDisplayed);
            }
            while (true);
        }

        private void UpdateBranchMenu(
            ref ModifiedFiles branches,
            ref List<string> leftSide,
            KeyInteractor<IElement> keyInteractor,
            List<string> mainPage,
            string branchesNames)
        {
            var height = consoleWrapper.WindowHeight;
            var width = consoleWrapper.WindowWidth;
            var boxHeight = (int)((height - 2) * 0.75);
            var boxWidth = (int)((width - 2) * 0.75);
            List<string> leftSideTemp;
            List<string> box;
            var commitList = new List<Commit>();
            var rightSide = new List<string>();
            foreach (var branch in branchesNames.Split("\n"))
            {
                commitList.Add(new Commit(gitManager.GetLastCommitMadeInBranch(branch.StartsWith("*") ? branch[2..] : branch)));
                var commit = commitList[^1].GetElement().Split("\n");
                rightSide.Add(new PageUtilities(commit[0] + " " + commit[3]).TruncateText((int)(0.6 * (height < 15 && width < 60 ? width - 1 : boxWidth))) + "\x1b[0m");
            }

            if (height < 15 && width < 60)
            {
                (branches.Index, branches.Current) = keyInteractor.KeySwitch(leftSide, leftSide[^1], leftSide[0], false, (int)(0.4 * width - 1), branches[branches.Current].GetElement().Length);
                leftSide = branches.GetHighlightedSide(height - 2, (int)(0.4 * width - 1), (height - 2) * (int)(0.4 * width - 2));
                leftSideTemp = new PageUtilities(leftSide.ToArray()).PadElement((int)(0.4 * width - 1));
                box = new BoxDrawer(MergeSides(leftSideTemp, rightSide, true), "Branches").DrawBox(width, height, false, true);
                UpdateWriter(box, height < 3 && width < 50);
                return;
            }

            (branches.Index, branches.Current) = keyInteractor.KeySwitch(leftSide, leftSide[^1], leftSide[0], false, (int)(0.4 * boxWidth - 1), branches[branches.Current].GetElement().Length);
            leftSide = branches.GetHighlightedSide(boxHeight - 2, (int)(0.4 * boxWidth - 1), (boxHeight - 2) * (int)(0.4 * boxWidth - 1));
            leftSideTemp = new PageUtilities(leftSide.ToArray()).PadElement((int)(0.4 * boxWidth - 1));
            box = new BoxDrawer(MergeSides(leftSideTemp, rightSide, true), "Branches").DrawBox(boxWidth, boxHeight, false, true);
            UpdateWriter(mainPage, height < 3 && width < 50, box);
        }

        private void DisplayCommitPage(Menu backgroundMenu, ModifiedFiles staged)
        {
            if (staged[0].GetElement() == "")
            {
                return;
            }

            var isWindowToSmall = consoleWrapper.WindowHeight < 8 && consoleWrapper.WindowWidth < 35;
            backgroundMenu.UpdateMenu(isWindowToSmall, key);
            var mainPage = backgroundMenu.LastPageDisplayed;
            var windowSize = 0;
            var windowSizeTemp = 0;
            var commitMessage = "";
            var keyInfo = new ConsoleKeyInfo((char)ConsoleKey.NoName, ConsoleKey.NoName, false, false, false);
            var commitMenu = new CommitMenu(consoleWrapper);
            commitMenu.UpdateMenu(isWindowToSmall, key);
            var commitBox = commitMenu.LastPageDisplayed;
            new MenuWriter(mainPage, isWindowToSmall, consoleWrapper).UpdateDisplay(WasWindowResizedSmaller, commitBox);

            do
            {
                if (keyInfo.Key == ConsoleKey.Escape)
                {
                    break;
                }
                else if (keyInfo.Key == ConsoleKey.Enter)
                {
                    commitMessage = commitMessage.Replace("\n", "");
                    gitManager.Commit(commitMessage);
                    commits = new FullCommits(gitManager.GetLog());
                    break;
                }

                if (WasWindowResizedSmaller)
                {
                    backgroundMenu.UpdateMenu(isWindowToSmall, key);
                }

                mainPage = backgroundMenu.LastPageDisplayed;
                commitMenu = new CommitMenu(consoleWrapper);
                commitMenu.UpdateMenu(isWindowToSmall, key);
                commitBox = commitMenu.LastPageDisplayed;
                var commitBoxToArray = commitBox.Split(Environment.NewLine);
                commitMessage = commitMessage[..Math.Min((commitBoxToArray[0].Length - 13) * (commitBox.Length - 3), commitMessage.Length)];
                new MenuWriter(mainPage, isWindowToSmall, consoleWrapper).UpdateDisplay(WasWindowResizedSmaller, commitBox);
                var height = consoleWrapper.WindowHeight;
                var width = consoleWrapper.WindowWidth;
                var left = (int)(0.125 * width) + 1;
                var top = (int)(0.125 * height) + 1;
                if (height < 15 && width < 60)
                {
                    left = 1;
                    top = 1;
                }

                (commitMessage, keyInfo) = new TextReader(left, top, commitBoxToArray[0].Length - 13, commitBoxToArray.Length - 2, consoleWrapper)
                    .GetText(ref commitMessage, keyInfo);
                key = keyInfo.Key;
                windowSize = height * width;
                WasWindowResizedSmaller = windowSizeTemp > windowSize;
                windowSizeTemp = windowSize;
            }
            while (true);
        }

        private void UpdateCommitPage(ref string commitMessage, List<string> mainPage, ref ConsoleKeyInfo keyInfo)
        {
            const string commitMenu = "\u001b[37m'Esc' Return \u001b[38[←] Commit";
            var height = consoleWrapper.WindowHeight;
            var width = consoleWrapper.WindowWidth;
            if (height < 15 && width < 60)
            {
                var box = new BoxDrawer(new List<string>(), "Commit").DrawBox(width, height, false, true, "", commitMenu);
                commitMessage = commitMessage[..Math.Min((box[0].Length - 13) * (box.Count - 2), commitMessage.Length)];
                UpdateWriter(box, consoleWrapper.WindowHeight < 3, box);
                (commitMessage, keyInfo) = new TextReader(1, 1, box[0].Length - 13, box.Count - 2, consoleWrapper).GetText(ref commitMessage, keyInfo);
            }
            else
            {
                var left = (int)(0.125 * width) + 1;
                var top = (int)(0.125 * height) + 1;
                var boxHeight = (int)(height * 0.75 - 2);
                var boxWidth = (int)(width * 0.75 - 2);
                var box = new BoxDrawer(new List<string>(), "Commit").DrawBox(boxWidth, boxHeight, false, true, "", commitMenu);
                commitMessage = commitMessage[..Math.Min((box[0].Length - 13) * (box.Count - 3), commitMessage.Length)];
                UpdateWriter(mainPage, consoleWrapper.WindowHeight < 3, box);
                (commitMessage, keyInfo) = new TextReader(left, top, box[0].Length - 13, box.Count - 2, consoleWrapper).GetText(ref commitMessage, keyInfo);
            }
        }
    }
}