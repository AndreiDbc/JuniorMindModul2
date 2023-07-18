using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitApp.Tests
{
    public class BoxDrawerTests
    {
        [Fact]
        public void DrawBoxIsCalledShouldDrawBox()
        {
            var text = new List<string>() { "asdasdsad7", "\x1b[7msdafdsfgdd", "0123456789" };
            var box = new BoxDrawer(text, "Diff").DrawBox(12, 7);
            var expected = new List<string>()
            {
                "┌Diff──────┐",
                "\u001b[0m│asdasdsad7\u001b[0m│",
                "\u001b[0m│\u001b[7msdafdsfgdd\u001b[0m│",
                "\u001b[0m│0123456789\u001b[0m│",
                "\u001b[0m│          \u001b[0m│",
                "\u001b[0m│          \u001b[0m│",
                "\u001b[0m└──────────┘"
            };
            Assert.Equal(expected, box);
        }
    }
}
