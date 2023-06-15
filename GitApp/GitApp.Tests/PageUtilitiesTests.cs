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
        public void GetFormatingCodesLengthAndStringWithoutCodesIsCalledElementDoesntContainAnyCodesShouldReturnLengthZero()
        {
            Assert.Equal(0, new PageUtilities("asa sasas ").GetFormatingCodesLengthAndStringWithoutCodes().length);
        }

        [Fact]
        public void GetFormatingCodesLengthAndStringWithoutCodesIsCalledShouldReturnCodesLength()
        {
            Assert.Equal(14, new PageUtilities(" \u001b[0masdasd\nasd \u001b[48;5;27mdsd d").GetFormatingCodesLengthAndStringWithoutCodes().length);
        }

        [Fact]
        public void GetFormatingCodesLengthAndStringWithoutCodesIsCalledShouldReturnClearedString()
        {
            var value = new PageUtilities(" \u001b[0mzasdasd\nasd \u001b[48;5;27mzdsd d").GetFormatingCodesLengthAndStringWithoutCodes().clearedString;
            const string expected = " zasdasd\nasd zdsd d";
            Assert.Equal(expected, value);
        }
    }
}
