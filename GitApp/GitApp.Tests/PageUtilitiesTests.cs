using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitApp.Tests
{
    public class PageUtilitiesTests
    {
        [Fact]
        public void TruncateTextIsCalledShouldTruncateString()
        {
            Assert.Equal("asd...", new PageUtilities("asdasdafdf").TruncateText(6));
        }

        [Fact]
        public void TruncateTextIsCalledSizeIsLessThanThreeShouldTruncateText()
        {
            Assert.Equal("...", new PageUtilities("as").TruncateText(2));
        }

        [Fact]
        public void GetLinesSizeForRightBoxIsCalledShouldReturnNoOfLines()
        {
            Assert.Equal(4, new PageUtilities("asds").GetOccupancyRate(2));
        }

        [Fact]
        public void GetLinesSizeForRightBoxIsCalledRemainderIsGreaterThanZeroShouldReturnNoOfLines()
        {
            Assert.Equal(6, new PageUtilities("asasa").GetOccupancyRate(2));
        }

        [Fact]
        public void GetLinesSizeForRightBoxIsCalledElementContainsNewLinesShouldReturnNoOfLines()
        {
            Assert.Equal(8, new PageUtilities("as\nas\nsaa").GetOccupancyRate(2));
        }

        [Fact]
        public void FormatingCodesStringUtilitiesIsCalledElementDoesntContainAnyCodesShouldReturnLengthZero()
        {
            Assert.Equal(0, new PageUtilities("asa sasas ").FormatingCodesStringUtilities().length);
        }

        [Fact]
        public void FormatingCodesStringUtilitiesIsCalledShouldReturnCodesLength()
        {
            Assert.Equal(18, new PageUtilities(" \u001b[0masdasd\nasd \u001b[48;5;27mdsd\u001b[0m d").FormatingCodesStringUtilities().length);
        }

        [Fact]
        public void FormatingCodesStringUtilitiesIsCalledShouldReturnClearedString()
        {
            var value = new PageUtilities(" \u001b[0mzasdasd\nasd \u001b[48;5;27mzdsd d").FormatingCodesStringUtilities().clearedString;
            const string expected = " zasdasd\nasd zdsd d";
            Assert.Equal(expected, value);
        }

        [Fact]
        public void FormatingCodesStringUtilitiesIsCalledShouldReturnCodes()
        {
            var value = new PageUtilities(" \u001b[0mzasdasd\nasd \u001b[48;5;27mzdsd d").FormatingCodesStringUtilities().codes;
            const string expected = "\u001b[0m\u001b[48;5;27m";
            Assert.Equal(expected, value);
        }

        [Fact]
        public void SplitRowIsCalledShouldSplitStrings()
        {
            var value = new PageUtilities(new[] { "asdafsasdfdsf", "asdg" }).SplitRow(3);
            var expected = new List<string>() 
            {
                "asd" + "\u001b[0m",
                "afs" + "\u001b[0m",
                "asd" + "\u001b[0m",
                "fds" + "\u001b[0m",
                "f" + "\u001b[0m",
                "asd" + "\u001b[0m",
                "g" + "\u001b[0m"
            };
            Assert.Equal(expected, value);
        }

        [Fact]
        public void SplitRowIsCalledStringContainsFormatingCodesShouldSplitStrings()
        {
            var value = new PageUtilities(new[] { "\x1b[38;5;190m" + "asdhadfgoiur t ", "rftyhfgh 433rrgf", " pdfopdfj fdneef", "\x1b[38;5;190m" + "odjcmndfkdjhrjhr" }).SplitRow(11);
            var expected = new List<string>() 
            { 
                "\x1b[38;5;190m" + "asdhadfgoiu" + "\u001b[0m",
                "\x1b[38;5;190m" + "r t " + "\u001b[0m",
                "rftyhfgh 43" + "\u001b[0m", "3rrgf" + "\u001b[0m",
                " pdfopdfj f" + "\u001b[0m", "dneef" + "\u001b[0m",
                "\x1b[38;5;190m" + "odjcmndfkdj" + "\u001b[0m",
                "\x1b[38;5;190m" + "hrjhr" + "\u001b[0m"
            };
            Assert.Equal(expected, value);
        }
    }
}
