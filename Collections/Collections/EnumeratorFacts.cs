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
            object[] array = new object[] { 1, 2, 3 };
            Enumerator enumerator = new Enumerator(array);
            Assert.True(enumerator.MoveNext());
        }
    }
}
