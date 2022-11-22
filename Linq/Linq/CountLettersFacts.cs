using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Linq
{
    public class CountLettersFacts
    {
        [Fact]
        public void CountVocalsIsCalledShouldReturnNoOfVocals()
        {
            Assert.Equal(3, new CountLetters("fhAgou").CountVocals());
        }
    }
}
