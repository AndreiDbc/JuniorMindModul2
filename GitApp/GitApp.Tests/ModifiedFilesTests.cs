using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitApp.Tests
{
    public class ModifiedFilesTests
    {
        [Fact]
        public void GetTextIsCalledShouldReturnText()
        {
            var modifiedFiles = new ModifiedFiles(
                "GitApp/GitApp/DisplayText.cs\n" +
                "GitApp/GitApp/ConsoleWrapper.cs\n");
            var output = modifiedFiles.GetText();
            Assert.Equal("GitApp/GitApp/DisplayText.cs", output[0].GetElement());
            Assert.Equal("GitApp/GitApp/ConsoleWrapper.cs", output[1].GetElement());
        }

        [Fact]
        public void GetPageDataAndSizeIsCalledShouldReturnTextAndSize()
        {
            var modifiedFiles = new ModifiedFiles(
                "GitApp/GitApp/DisplayText.cs\n" +
                "GitApp/GitApp/ConsoleWrapper.cs\n" +
                "GitApp/GitApp/IElement.cs\n" +
                "GitApp/GitApp/Program.cs\n" +
                "GitApp/GitApp/InitialData.cs\n" +
                "GitApp/GitApp/IInteractor.cs\n" +
                "GitApp/GitApp.Tests/DisplayTextTests.cs\n" +
                "GitApp/GitApp.Tests/FakeConsole.cs");
            var output = modifiedFiles.GetPageDataAndSize(1960, 70);
            Assert.Equal("/DisplayText.cs", output.list[0].Id);
            Assert.Equal("/ConsoleWrapper.cs", output.list[1].Id);
            Assert.Equal("/IElement.cs", output.list[2].Id);
            Assert.Equal("/Program.cs", output.list[3].Id);
            Assert.Equal("/InitialData.cs", output.list[4].Id);
            Assert.Equal("/IInteractor.cs", output.list[5].Id);
            Assert.Equal("/DisplayTextTests.cs", output.list[6].Id);
            Assert.Equal("/FakeConsole.cs", output.list[7].Id);
            Assert.Equal(560, output.textSize);
        }

        [Fact]
        public void GetHighlightedSideIsCalledShouldReturnText()
        {
            var modifiedFiles = new ModifiedFiles(
                "GitApp/GitApp/DisplayText.cs\n" +
                "GitApp/GitApp/ConsoleWrapper.cs\n" +
                "GitApp/GitApp/IElement.cs\n" +
                "GitApp/GitApp/Program.cs\n" +
                "GitApp/GitApp/InitialData.cs\n" +
                "GitApp/GitApp/IInteractor.cs\n" +
                "GitApp/GitApp.Tests/DisplayTextTests.cs\n" +
                "GitApp/GitApp.Tests/FakeConsole.cs");
            var output = modifiedFiles.GetHighlightedSide(30, 70, 1960);
            var expected = new List<string>()
            {
                "\u001b[7mGitApp/GitApp/DisplayText.cs",
                "\u001b[0mGitApp/GitApp/ConsoleWrapper.cs",
                "\u001b[0mGitApp/GitApp/IElement.cs",
                "\u001b[0mGitApp/GitApp/Program.cs",
                "\u001b[0mGitApp/GitApp/InitialData.cs",
                "\u001b[0mGitApp/GitApp/IInteractor.cs",
                "\u001b[0mGitApp/GitApp.Tests/DisplayTextTests.cs",
                "\u001b[0mGitApp/GitApp.Tests/FakeConsole.cs"
            };
            Assert.Equal(expected, output);

        }
    }
}
