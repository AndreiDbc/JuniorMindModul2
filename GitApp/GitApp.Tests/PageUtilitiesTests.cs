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
            Assert.Equal("asd...", PageUtilities.TruncateText("asdasdafdf", 6));
        }

        [Fact]
        public void GetSizeIsCalledElementDoesntContainFormatingCodesShouldReturnPageSize()
        {
            var page = new List<string>() { "asd .", "sds", "ewqa" };
            Assert.Equal(360, PageUtilities.GetSize(page));
        }
    }
}
