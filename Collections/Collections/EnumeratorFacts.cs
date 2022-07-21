using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Collections
{
    public class EnumeratorFacts
    {
        [Fact]
        public void MoveNextIsCalledShouldReturnTrue()
        {
            List array = new () { 1, 2, 3 };
            Enumerator enumerator = new Enumerator(array);
            Assert.True(enumerator.MoveNext());
        }

        [Fact]
        public void CurrentNextIsCalledShouldReturnTrue()
        {
            List array = new () { 1, 2, 3 };
            Enumerator enumerator = new Enumerator(array);
            enumerator.MoveNext();
            Assert.Equal(1, enumerator.Current);
        }
    }
}
