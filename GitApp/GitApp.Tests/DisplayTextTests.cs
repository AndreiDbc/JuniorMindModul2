using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace GitApp.Tests
{
    public class DisplayTextTests
    {
        const string Commits = "commit 7f37022a20e43184a99dc063be55a27ff5afb34d (HEAD -> master)" +
                "\nAuthor: Andrei Daboc <99188105+AndreiDbc@users.noreply.github.com>" +
                "\nDate:   Wed May 17 20:46:13 2023 +0300" +
                "\n\n    Removed temp variable from FitTextInsideBox method." +
                "\n\ncommit bfa2684c13f78acf0468d2b9d459ff1021327d18" +
                "\nAuthor: Andrei Daboc <99188105+AndreiDbc@users.noreply.github.com>" +
                "\nDate:   Wed May 17 20:09:35 2023 +0300" +
                "\n\n    Fixed an issue that caused TruncateText method to be called when not necessary." +
                "\n\ncommit 1db76a24c52ba7c952fa9e47e57e77fcfbedd3dd" +
                "\nAuthor: Andrei Daboc <99188105+AndreiDbc@users.noreply.github.com>" +
                "\nDate:   Wed May 17 12:04:21 2023 +0300" +
                "\n\n    Data is now displayed on the whole console window." +
                "\n\ncommit adfdf0eb554c0e40ce13bb8c5a9b4612a62c6045 (origin/master, origin/HEAD)" +
                "\nAuthor: Andrei Daboc <99188105+AndreiDbc@users.noreply.github.com>" +
                "\nDate:   Fri May 12 12:32:46 2023 +0300" +
                "\n\n    Removed FormateRightSide's \"limit\" parameter." +
                "\n\ncommit f944893636d43d1bf1468c41202ce194bf8d25e3" +
                "\nAuthor: Andrei Daboc <99188105+AndreiDbc@users.noreply.github.com>" +
                "\nDate:   Fri May 12 12:27:52 2023 +0300" +
                "\n\n    Boxes are now properly displayed at any size." +
                "\n\ncommit 92e239d63095136fc2811956043938ddb3a1e4fe" +
                "\nAuthor: Andrei Daboc <99188105+AndreiDbc@users.noreply.github.com>" +
                "\nDate:   Tue May 9 19:49:54 2023 +0300" +
                "\n\n    FitTextInsideBox method's loop now breaks if \"i\" is equal to current line element's length minus one." +
                "\n\ncommit 10580c965b789e1ef4c1d588530903f7b82e9136" +
                "\nAuthor: Andrei Daboc <99188105+AndreiDbc@users.noreply.github.com>" +
                "\nDate:   Tue May 9 19:48:11 2023 +0300" +
                "\n\n    Modified functionality of DisplaySplitScreen method." +
                "\n\ncommit fae78e6f1f66c2a85d89871dc053bc90c269ddbd" +
                "\nAuthor: Andrei Daboc <99188105+AndreiDbc@users.noreply.github.com>" +
                "\nDate:   Tue May 9 13:34:45 2023 +0300" +
                "\n\n    Fixed date column arrangement on left side." +
                "\n\ncommit f4ad2943cab64ca72b7ab5fa194a81183509b7bf" +
                "\nAuthor: Andrei Daboc <99188105+AndreiDbc@users.noreply.github.com>" +
                "\nDate:   Tue May 9 13:25:32 2023 +0300" +
                "\n\n    GetLeftSide method's leftSideColumnSize variable is now 15% of console window's width." +
                "\n\ncommit d3d8ef77607f81818f50e846a49b05623ab8fa68" +
                "\nAuthor: Andrei Daboc <99188105+AndreiDbc@users.noreply.github.com>" +
                "\nDate:   Mon May 8 14:37:15 2023 +0300" +
                "\n\n    Fixed an issue that caused not to display the correct date.\r\n";

        const string initialText = "Current branch name: master\r\nOutgoing: 0\r\nIncoming: 0\r\nBranches: * master";
        const string expectedMainPage =
                "Current branch name: master\r                                                                                            " +
                "\r\nOutgoing: 0\r                                                                                                            " +
                "\r\nIncoming: 0\r                                                                                                            " +
                "\r\nBranches: * master                                                                                                      " +
                "\r\n\u001b[7m\u001b[48;5;27m7f37022 \u001b[48;5;46mAndrei Daboc \u001b[48;5;196m2023-05-17 \u001b[0m\u001b[7mRemoved temp variable from FitTextInsideBox method.\u001b[0m                                     " +
                "\r\n\u001b[0m\u001b[38;5;27mbfa2684 \u001b[38;5;46mAndrei Daboc \u001b[38;5;196m2023-05-17 \u001b[0mFixed an issue that caused TruncateText method to be called when not necessary.\u001b[0m         " +
                "\r\n\u001b[0m\u001b[38;5;27m1db76a2 \u001b[38;5;46mAndrei Daboc \u001b[38;5;196m2023-05-17 \u001b[0mData is now displayed on the whole console window.\u001b[0m                                      " +
                "\r\n\u001b[0m\u001b[38;5;27madfdf0e \u001b[38;5;46mAndrei Daboc \u001b[38;5;196m2023-05-12 \u001b[0mRemoved FormateRightSide's \"limit\" parameter.\u001b[0m                                           " +
                "\r\n\u001b[0m\u001b[38;5;27mf944893 \u001b[38;5;46mAndrei Daboc \u001b[38;5;196m2023-05-12 \u001b[0mBoxes are now properly displayed at any size.\u001b[0m                                           " +
                "\r\n\u001b[0m\u001b[38;5;27m92e239d \u001b[38;5;46mAndrei Daboc \u001b[38;5;196m2023-05-09 \u001b[0mFitTextInsideBox method's loop now breaks if \"i\" is equal to current line element's length minus one.\u001b[0m                                                                                                           " +
                "\r\n\u001b[0m\u001b[38;5;27m10580c9 \u001b[38;5;46mAndrei Daboc \u001b[38;5;196m2023-05-09 \u001b[0mModified functionality of DisplaySplitScreen method.\u001b[0m                                    " +
                "\r\n\u001b[0m\u001b[38;5;27mfae78e6 \u001b[38;5;46mAndrei Daboc \u001b[38;5;196m2023-05-09 \u001b[0mFixed date column arrangement on left side.\u001b[0m                                             " +
                "\r\n\u001b[0m\u001b[38;5;27mf4ad294 \u001b[38;5;46mAndrei Daboc \u001b[38;5;196m2023-05-09 \u001b[0mGetLeftSide method's leftSideColumnSize variable is now 15% of console window's width.\u001b[0m  " +
                "\r\n\u001b[0m\u001b[38;5;27md3d8ef7 \u001b[38;5;46mAndrei Daboc \u001b[38;5;196m2023-05-08 \u001b[0mFixed an issue that caused not to display the correct date.\u001b[0m\u001b[0m                             " +
                "\r\n\u001b[0mPress 'Esc' to exit.                                                                                                    \r";
        const string expectedExtendedPage =
                "┌Commits───────────────────────────────────────────────────────────────┐┌Info──────────────────────────────────────────┐" +
                "\r\n\u001b[0m│\u001b[7m\u001b[48;5;27m7f37022 \u001b[48;5;46mAndrei Daboc \u001b[48;5;196m2023-05-17 \u001b[0m\u001b[7mRemoved temp variable from FitTe...   \u001b[0m│\u001b[0m│Author: Andrei Daboc\u001b[0m                          │" +
                "\r\n\u001b[0m│\u001b[0m\u001b[38;5;27mbfa2684 \u001b[38;5;46mAndrei Daboc \u001b[38;5;196m2023-05-17 \u001b[0mFixed an issue that caused Trunc...   \u001b[0m│\u001b[0m│Date: 2023-05-17\u001b[0m                              │" +
                "\r\n\u001b[0m│\u001b[0m\u001b[38;5;27m1db76a2 \u001b[38;5;46mAndrei Daboc \u001b[38;5;196m2023-05-17 \u001b[0mData is now displayed on the who...   \u001b[0m│\u001b[0m│SHA: 7f37022a20e43184a99dc063be55a27ff5afb34d\u001b[0m │" +
                "\r\n\u001b[0m│\u001b[0m\u001b[38;5;27madfdf0e \u001b[38;5;46mAndrei Daboc \u001b[38;5;196m2023-05-12 \u001b[0mRemoved FormateRightSide's \"limi...   \u001b[0m│\u001b[0m│                                              │" +
                "\r\n\u001b[0m│\u001b[0m\u001b[38;5;27mf944893 \u001b[38;5;46mAndrei Daboc \u001b[38;5;196m2023-05-12 \u001b[0mBoxes are now properly displayed...   \u001b[0m│\u001b[0m│                                              │" +
                "\r\n\u001b[0m│\u001b[0m\u001b[38;5;27m92e239d \u001b[38;5;46mAndrei Daboc \u001b[38;5;196m2023-05-09 \u001b[0mFitTextInsideBox method's loop n...   \u001b[0m│\u001b[0m│                                              │" +
                "\r\n\u001b[0m│\u001b[0m\u001b[38;5;27m10580c9 \u001b[38;5;46mAndrei Daboc \u001b[38;5;196m2023-05-09 \u001b[0mModified functionality of Displa...   \u001b[0m│\u001b[0m│                                              │" +
                "\r\n\u001b[0m│\u001b[0m\u001b[38;5;27mfae78e6 \u001b[38;5;46mAndrei Daboc \u001b[38;5;196m2023-05-09 \u001b[0mFixed date column arrangement on...   \u001b[0m│\u001b[0m│                                              │" +
                "\r\n\u001b[0m│\u001b[0m\u001b[38;5;27mf4ad294 \u001b[38;5;46mAndrei Daboc \u001b[38;5;196m2023-05-09 \u001b[0mGetLeftSide method's leftSideCol...   \u001b[0m│\u001b[0m└──────────────────────────────────────────────┘" +
                "\r\n\u001b[0m│\u001b[0m\u001b[38;5;27md3d8ef7 \u001b[38;5;46mAndrei Daboc \u001b[38;5;196m2023-05-08 \u001b[0mFixed an issue that caused not t...   \u001b[0m│┌Message───────────────────────────────────────┐" +
                "\r\n\u001b[0m│                                                                      \u001b[0m│\u001b[0m│Removed temp variable from FitTextInsideBox me\u001b[0m│" +
                "\r\n\u001b[0m│                                                                      \u001b[0m│\u001b[0m│thod.\u001b[0m                                         │" +
                "\r\n\u001b[0m│                                                                      \u001b[0m│\u001b[0m│                                              │" +
                "\r\n\u001b[0m│                                                                      \u001b[0m│\u001b[0m│                                              │" +
                "\r\n\u001b[0m│                                                                      \u001b[0m│\u001b[0m│                                              │" +
                "\r\n\u001b[0m│                                                                      \u001b[0m│\u001b[0m│                                              │" +
                "\r\n\u001b[0m│                                                                      \u001b[0m│\u001b[0m│                                              │" +
                "\r\n\u001b[0m│                                                                      \u001b[0m│\u001b[0m│                                              │" +
                "\r\n\u001b[0m│                                                                      \u001b[0m│\u001b[0m└──────────────────────────────────────────────┘" +
                "\r\n\u001b[0m│                                                                      \u001b[0m│┌Files─────────────────────────────────────────┐" +
                "\r\n\u001b[0m│                                                                      \u001b[0m│\u001b[0m│GitApp/GitApp/DisplayText.cs\u001b[0m                  │" +
                "\r\n\u001b[0m│                                                                      \u001b[0m│\u001b[0m│GitApp/GitApp/ConsoleWrapper.cs\u001b[0m               │" +
                "\r\n\u001b[0m│                                                                      \u001b[0m│\u001b[0m│GitApp/GitApp/IElement.cs\u001b[0m                     │" +
                "\r\n\u001b[0m│                                                                      \u001b[0m│\u001b[0m│GitApp/GitApp/Program.cs\u001b[0m                      │" +
                "\r\n\u001b[0m│                                                                      \u001b[0m│\u001b[0m│GitApp/GitApp/InitialData.cs\u001b[0m                  │" +
                "\r\n\u001b[0m│                                                                      \u001b[0m│\u001b[0m│GitApp/GitApp/IInteractor.cs\u001b[0m                  │" +
                "\r\n\u001b[0m│                                                                      \u001b[0m│\u001b[0m│GitApp/GitApp.Tests/DisplayTextTests.cs\u001b[0m       │" +
                "\r\n\u001b[0m│                                                                      \u001b[0m│\u001b[0m│GitApp/GitApp.Tests/FakeConsole.cs\u001b[0m            │" +
                "\r\n\u001b[0m└──────────────────────────────────────────────────────────────────────┘\u001b[0m└──────────────────────────────────────────────┘\r";
        const string expectedSmallMainPage =
            "Current branch name: master\r                     " +
            "\r\nOutgoing: 0\r                                     " +
            "\r\nIncoming: 0\r                                     " +
            "\r\nBranches: * master                               " +
            "\r\n\u001b[7m\u001b[48;5;27m7f37022 \u001b[48;5;46mAndrei Daboc \u001b[48;5;196m2023-05-17 \u001b[0m\u001b[7mRemoved temp variable from FitTextInsideBox method.\u001b[0m               " +
            "\r\n\u001b[0m\u001b[38;5;27mbfa2684 \u001b[38;5;46mAndrei Daboc \u001b[38;5;196m2023-05-17 \u001b[0mFixed an issue that caused TruncateText method to be called when not necessary.\u001b[0m                                    " +
            "\r\n\u001b[0m\u001b[38;5;27m1db76a2 \u001b[38;5;46mAndrei Daboc \u001b[38;5;196m2023-05-17 \u001b[0mData is now displayed on the whole console window.\u001b[0m                " +
            "\r\n\u001b[0m\u001b[38;5;27madfdf0e \u001b[38;5;46mAndrei Daboc \u001b[38;5;196m2023-05-12 \u001b[0mRemoved FormateRightSide's \"limit\" parameter.\u001b[0m\u001b[0m                     " +
            "\r\n\u001b[0mPress 'Esc' to exit.                             \r";
        const string expectedSmallRightExtendedPage =
            "┌Info───────────────────────────────────────────┐" +
            "\r\n\u001b[0m│Author: Andrei Daboc\u001b[0m                           │" +
            "\r\n\u001b[0m│Date: 2023-05-17\u001b[0m...                            │" +
            "\r\n\u001b[0m└───────────────────────────────────────────────┘" +
            "\r\n┌Message────────────────────────────────────────┐" +
            "\r\n\u001b[0m│Removed temp variable from FitTextInsideBox met\u001b[0m│" +
            "\r\n\u001b[0m│hod.\u001b[0m                                           │" +
            "\r\n\u001b[0m│                                               │" +
            "\r\n\u001b[0m│                                               │" +
            "\r\n\u001b[0m└───────────────────────────────────────────────┘" +
            "\r\n┌Files──────────────────────────────────────────┐" +
            "\r\n\u001b[0m│GitApp/GitApp/DisplayText.cs\u001b[0m                   │" +
            "\r\n\u001b[0m│GitApp/GitApp/ConsoleWrapper.cs\u001b[0m...             │" +
            "\r\n\u001b[0m└───────────────────────────────────────────────┘\r";
        const string expectedSmallLeftExtendedPage =
            "┌Commits────────────────────────────────────────┐"+
            "\r\n\u001b[0m│\u001b[7m\u001b[48;5;27m7f37022 \u001b[48;5;46mAndrei ... \u001b[48;5;196m2023-0... \u001b[0m\u001b[7mRemoved tem...    \u001b[0m│" +
            "\r\n\u001b[0m│\u001b[0m\u001b[38;5;27mbfa2684 \u001b[38;5;46mAndrei ... \u001b[38;5;196m2023-0... \u001b[0mFixed an is...    \u001b[0m│" +
            "\r\n\u001b[0m│\u001b[0m\u001b[38;5;27m1db76a2 \u001b[38;5;46mAndrei ... \u001b[38;5;196m2023-0... \u001b[0mData is now...    \u001b[0m│" +
            "\r\n\u001b[0m│\u001b[0m\u001b[38;5;27madfdf0e \u001b[38;5;46mAndrei ... \u001b[38;5;196m2023-0... \u001b[0mRemoved For...    \u001b[0m│" +
            "\r\n\u001b[0m│\u001b[0m\u001b[38;5;27mf944893 \u001b[38;5;46mAndrei ... \u001b[38;5;196m2023-0... \u001b[0mBoxes are n...    \u001b[0m│" +
            "\r\n\u001b[0m│\u001b[0m\u001b[38;5;27m92e239d \u001b[38;5;46mAndrei ... \u001b[38;5;196m2023-0... \u001b[0mFitTextInsi...    \u001b[0m│" +
            "\r\n\u001b[0m│\u001b[0m\u001b[38;5;27m10580c9 \u001b[38;5;46mAndrei ... \u001b[38;5;196m2023-0... \u001b[0mModified fu...    \u001b[0m│" +
            "\r\n\u001b[0m│\u001b[0m\u001b[38;5;27mfae78e6 \u001b[38;5;46mAndrei ... \u001b[38;5;196m2023-0... \u001b[0mFixed date ...    \u001b[0m│" +
            "\r\n\u001b[0m│\u001b[0m\u001b[38;5;27mf4ad294 \u001b[38;5;46mAndrei ... \u001b[38;5;196m2023-0... \u001b[0mGetLeftSide...    \u001b[0m│" +
            "\r\n\u001b[0m│\u001b[0m\u001b[38;5;27md3d8ef7 \u001b[38;5;46mAndrei ... \u001b[38;5;196m2023-0... \u001b[0mFixed an is...    \u001b[0m│" +
            "\r\n\u001b[0m│                                               \u001b[0m│" +
            "\r\n\u001b[0m│                                               \u001b[0m│" +
            "\r\n\u001b[0m└───────────────────────────────────────────────┘\r";
        const string expectedDiffPage =
            "┌Files─────────────────────────────────────────────────────────────────┐┌Diff──────────────────────────────────────────┐"+
            "\r\n\u001b[0m│\u001b[7mGitApp/GitApp/DisplayText.cs                                          \u001b[0m│\u001b[0m│diff --git a/GitApp/GitApp/DisplayText.cs b/Gi\u001b[0m│" +
            "\r\n\u001b[0m│\u001b[0mGitApp/GitApp/ConsoleWrapper.cs                                       \u001b[0m│\u001b[0m│tApp/GitApp/DisplayText.cs\r\u001b[0m                   │" +
            "\r\n\u001b[0m│\u001b[0mGitApp/GitApp/IElement.cs                                             \u001b[0m│\u001b[0m│index 2e671a6..0802a33 100644\r\u001b[0m                │" +
            "\r\n\u001b[0m│\u001b[0mGitApp/GitApp/Program.cs                                              \u001b[0m│\u001b[0m│--- a/GitApp/GitApp/DisplayText.cs\r\u001b[0m           │" +
            "\r\n\u001b[0m│\u001b[0mGitApp/GitApp/InitialData.cs                                          \u001b[0m│\u001b[0m│+++ b/GitApp/GitApp/DisplayText.cs\r\u001b[0m           │" +
            "\r\n\u001b[0m│\u001b[0mGitApp/GitApp/IInteractor.cs                                          \u001b[0m│\u001b[0m│@@ -192,11 +192,20 @@ namespace GitApp\r\u001b[0m       │" +
            "\r\n\u001b[0m│\u001b[0mGitApp/GitApp.Tests/DisplayTextTests.cs                               \u001b[0m│\u001b[0m│         private static List<string> SplitRow(\u001b[0m│" +
            "\r\n\u001b[0m│\u001b[0mGitApp/GitApp.Tests/FakeConsole.cs                                    \u001b[0m│\u001b[0m│string[] data, int limit)\r\u001b[0m                    │" +
            "\r\n\u001b[0m│                                                                      \u001b[0m│\u001b[0m│         {\r\u001b[0m                                   │" +
            "\r\n\u001b[0m│                                                                      \u001b[0m│\u001b[0m│             var result = new List<string>();\r\u001b[0m│" +
            "\r\n\u001b[0m│                                                                      \u001b[0m│\u001b[0m│\u001b[38;5;40m+            var codes = \"\";\r\u001b[0m                 │" +
            "\r\n\u001b[0m│                                                                      \u001b[0m│\u001b[0m│             for (int i = 0; i < data.Length; \u001b[0m│" +
            "\r\n\u001b[0m│                                                                      \u001b[0m│\u001b[0m│i++)\r\u001b[0m                                         │" +
            "\r\n\u001b[0m│                                                                      \u001b[0m│\u001b[0m│             {\r\u001b[0m                               │" +
            "\r\n\u001b[0m│                                                                      \u001b[0m│\u001b[0m│\u001b[38;5;190m-                for (int j = 0, k = 1; j < da\u001b[0m│" +
            "\r\n\u001b[0m│                                                                      \u001b[0m│\u001b[0m│\u001b[38;5;190mta[i].Length; j += limit, k++)\r\u001b[0m               │" +
            "\r\n\u001b[0m│                                                                      \u001b[0m│\u001b[0m│\u001b[38;5;190m+                var differenceLength = 0;\r\u001b[0m   │" +
            "\r\n\u001b[0m│                                                                      \u001b[0m│\u001b[0m│\u001b[38;5;40m+                codes = \"\";\r\u001b[0m                 │" +
            "\r\n\u001b[0m│                                                                      \u001b[0m│\u001b[0m│\u001b[38;5;40m+                for (int j = 0, k = 1; j < da\u001b[0m│" +
            "\r\n\u001b[0m│                                                                      \u001b[0m│\u001b[0m│\u001b[38;5;40mta[i].Length; j += limit + differenceLength, k\u001b[0m│" +
            "\r\n\u001b[0m│                                                                      \u001b[0m│\u001b[0m│\u001b[38;5;40m++)\r\u001b[0m                                          │" +
            "\r\n\u001b[0m│                                                                      \u001b[0m│\u001b[0m│                 {\r\u001b[0m                           │" +
            "\r\n\u001b[0m│                                                                      \u001b[0m│\u001b[0m│\u001b[38;5;190m-                    result.Add(data[i][j..Mat\u001b[0m│" +
            "\r\n\u001b[0m│                                                                      \u001b[0m│\u001b[0m│\u001b[38;5;190mh.Min(data[i].Length, limit * k)]);\r\u001b[0m          │" +
            "\r\n\u001b[0m│                                                                      \u001b[0m│\u001b[0m│\u001b[38;5;190m+                    var element = data[i][j..\u001b[0m│" +
            "\r\n\u001b[0m│                                                                      \u001b[0m│\u001b[0m│\u001b[38;5;190mMath.Min(data[i].Length, limit * k)];\r\u001b[0m        │" +
            "\r\n\u001b[0m│                                                                      \u001b[0m│\u001b[0m│\u001b[38;5;40m+                    var elementUtilities = ne\u001b[0m│" +
            "\r\n\u001b[0m│                                                                      \u001b[0m│\u001b[0m│\u001b[38;5;40mw PageUtilities(element);\r\u001b[0m...                 │" +
            "\r\n\u001b[0m└──────────────────────────────────────────────────────────────────────┘\u001b[0m└──────────────────────────────────────────────┘\r";

        [Fact]
        public void DisplayIsCalledShouldDisplayMainText()
        {
            ConsoleKeyInfo[] keyinfo = new[] { new ConsoleKeyInfo((char)ConsoleKey.Escape, ConsoleKey.Escape, false, false, false) };
            StringWriter writer = new StringWriter();
            var fakeConsole = new FakeConsole(keyinfo, writer);
            fakeConsole.WindowHeight = 30;
            fakeConsole.WindowWidth = 120;
            var manager = new GitManager("git", new FakeGitProcessRunner());
            var display = new DisplayText(new FullCommits(Commits), writer, manager, fakeConsole);
            display.Display(initialText);
            var actualOutput = writer.ToString();
            Assert.Equal(expectedMainPage + expectedMainPage, actualOutput);
        }

        [Fact]
        public void DisplayIsCalledEnterIsPressedShouldDisplayExtendedPage()
        {
            ConsoleKeyInfo[] keySequence = new[] 
            {
                new ConsoleKeyInfo((char)ConsoleKey.Enter, ConsoleKey.Enter, false, false, false)
            };
            StringWriter writer = new StringWriter();
            var fakeConsole = new FakeConsole(keySequence, writer);
            fakeConsole.WindowHeight = 30;
            fakeConsole.WindowWidth = 120;
            var manager = new GitManager("git", new FakeGitProcessRunner());
            var display = new DisplayText(new FullCommits(Commits), writer, manager, fakeConsole);
            display.Display(initialText);
            var actualOutput = writer.ToString();
            Assert.Equal(expectedMainPage + expectedExtendedPage + expectedExtendedPage + expectedMainPage + expectedMainPage, actualOutput);
        }

        [Fact]
        public void DisplayIsCalledEnterIsPressedFollowedByBackSpaceWhileWindowIsSmallShouldDisplayText()
        {
            ConsoleKeyInfo[] keySequence = new[]
{
                new ConsoleKeyInfo((char)ConsoleKey.Enter, ConsoleKey.Enter, false, false, false),
                new ConsoleKeyInfo((char)ConsoleKey.Spacebar, ConsoleKey.Spacebar, false, false, false)
            };
            StringWriter writer = new StringWriter();
            var fakeConsole = new FakeConsole(keySequence, writer);
            fakeConsole.WindowHeight = 14;
            fakeConsole.WindowWidth = 49;
            var manager = new GitManager("git", new FakeGitProcessRunner());
            var display = new DisplayText(new FullCommits(Commits), writer, manager, fakeConsole);
            display.Display(initialText);
            var actualOutput = writer.ToString();
            Assert.Equal(expectedSmallMainPage +
                expectedSmallLeftExtendedPage +
                expectedSmallRightExtendedPage +
                expectedSmallRightExtendedPage +
                expectedSmallMainPage +
                expectedSmallMainPage,
                actualOutput);
        }

        [Fact]
        public void DisplayIsCalledEnterFollowedByRightArrowIsPressedShouldDisplayDiffPage()
        {
            ConsoleKeyInfo[] keySequence = new[]
            {
                new ConsoleKeyInfo((char)ConsoleKey.Enter, ConsoleKey.Enter, false, false, false),
                new ConsoleKeyInfo((char)ConsoleKey.RightArrow, ConsoleKey.RightArrow, false, false, false)
            };
            StringWriter writer = new StringWriter();
            var fakeConsole = new FakeConsole(keySequence, writer);
            fakeConsole.WindowHeight = 30;
            fakeConsole.WindowWidth = 120;
            var manager = new GitManager("git", new FakeGitProcessRunner());
            var display = new DisplayText(new FullCommits(Commits), writer, manager, fakeConsole);
            display.Display(initialText);
            var actualOutput = writer.ToString();
            Assert.Equal(
                expectedMainPage+
                expectedExtendedPage+
                expectedDiffPage+
                expectedDiffPage+
                expectedExtendedPage+
                expectedExtendedPage +
                expectedMainPage +
                expectedMainPage,
                actualOutput);
        }
    }
}
