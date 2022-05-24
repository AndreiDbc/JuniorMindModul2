using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace JsonSecondVersion
{
    public class SequenceFacts
    {
        [Fact]
        public void SequenceIsGivenAMatchShouldReturnRemainingText()
        {
            var hex = new Choice(new Range('0', '9'), new Range('a', 'f'), new Range('A', 'F'));
            var hexSeq = new Sequence(new Character('u'), new Sequence(hex, hex, hex, hex));
            Assert.Equal("asd", hexSeq.Match("u1234asd").RemainingText());
        }

        [Fact]
        public void SequenceIsGivenAMatchShouldReturnTrue()
        {
            var hex = new Choice(new Range('0', '9'), new Range('a', 'f'), new Range('A', 'F'));
            var hexSeq = new Sequence(new Character('u'), new Sequence(hex, hex, hex, hex));
            Assert.True(hexSeq.Match("u1234").Success());
        }

        [Fact]
        public void SequenceIsGivenANonMatchShouldReturnFullText()
        {
            var hex = new Choice(new Range('0', '9'), new Range('a', 'f'), new Range('A', 'F'));
            var hexSeq = new Sequence(new Character('u'), new Sequence(hex, hex, hex, hex));
            Assert.Equal("uasd", hexSeq.Match("uasd").RemainingText());
        }

        [Fact]
        public void SequenceIsGivenANonMatchShouldReturnFalse()
        {
            var hex = new Choice(new Range('0', '9'), new Range('a', 'f'), new Range('A', 'F'));
            var hexSeq = new Sequence(new Character('u'), new Sequence(hex, hex, hex, hex));
            Assert.False(hexSeq.Match("uasd").Success());
        }
    }
}
