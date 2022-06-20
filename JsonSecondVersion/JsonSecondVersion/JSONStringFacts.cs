using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace JsonSecondVersion
{
    public class JSONStringFacts
    {
        public static string Quoted(string text)
            => $"\"{text}\"";

        [Fact]
        public void IsWrappedInDoubleQuotes()
        {
            JSONString json = new ();
            Assert.True(json.Match(Quoted("abc")).Success());
        }

        [Fact]
        public void AlwaysStartsWithQuotes()
        {
            JSONString json = new ();
            Assert.False(json.Match("abc\"").Success());
        }

        [Fact]
        public void AlwaysEndsWithQuotes()
        {
            JSONString json = new ();
            Assert.False(json.Match("\"abc").Success());
        }

        [Fact]
        public void IsNotAnEmptyString()
        {
            JSONString json = new ();
            Assert.False(json.Match(string.Empty).Success());
        }

        [Fact]
        public void IsAnEmptyDoubleQuotedString()
        {
            JSONString json = new ();
            Assert.True(json.Match(Quoted(string.Empty)).Success());
        }

        [Fact]
        public void HasStartAndEndQuotes()
        {
            JSONString json = new ();
            Assert.False(json.Match("\"").Success());
        }

        [Fact]
        public void DoesNotContainControlCharacters()
        {
            JSONString json = new ();
            Assert.False(json.Match(Quoted("a\nb\rc")).Success());
        }

        [Fact]
        public void CanContainLargeUnicodeCharacters()
        {
            JSONString json = new ();
            Assert.True(json.Match(Quoted("⛅⚾")).Success());
        }

        [Fact]
        public void CanContainEscapedQuotationMark()
        {
            JSONString json = new ();
            Assert.True(json.Match(Quoted(@"\""a\"" b")).Success());
        }

        [Fact]
        public void CanContainEscapedReverseSolidus()
        {
            JSONString json = new ();
            Assert.True(json.Match(Quoted(@"a \\ b")).Success());
        }

        [Fact]
        public void CanContainEscapedSolidus()
        {
            JSONString json = new ();
            Assert.True(json.Match(Quoted(@"a \/ b")).Success());
        }

        [Fact]
        public void CanContainEscapedBackspace()
        {
            JSONString json = new ();
            Assert.True(json.Match(Quoted(@"a \b b")).Success());
        }

        [Fact]
        public void CanContainEscapedFormFeed()
        {
            JSONString json = new ();
            Assert.True(json.Match(Quoted(@"a \f b")).Success());
        }

        [Fact]
        public void CanContainEscapedLineFeed()
        {
            JSONString json = new ();
            Assert.True(json.Match(Quoted(@"a \n b")).Success());
        }

        [Fact]
        public void CanContainEscapedCarrigeReturn()
        {
            JSONString json = new ();
            Assert.True(json.Match(Quoted(@"a \r b")).Success());
        }

        [Fact]
        public void CanContainEscapedHorizontalTab()
        {
            JSONString json = new ();
            Assert.True(json.Match(Quoted(@"a \t b")).Success());
        }

        [Fact]
        public void CanContainEscapedUnicodeCharacters()
        {
            JSONString json = new ();
            Assert.True(json.Match(Quoted(@"a \u26Be b")).Success());
        }

        [Fact]
        public void CanContainEscapedUnicodeCharactersShouldReturnEmptyString()
        {
            JSONString json = new ();
            Assert.Equal("", json.Match(Quoted(@"a \u26Be b")).RemainingText());
        }

        [Fact]
        public void DoesNotContainUnrecognizedExcapceCharacters()
        {
            JSONString json = new ();
            Assert.False(json.Match(Quoted(@"a\x")).Success());
        }

        [Fact]
        public void DoesNotEndWithReverseSolidus()
        {
            JSONString json = new ();
            Assert.False(json.Match(Quoted(@"a\")).Success());
        }

        [Fact]
        public void DoesNotEndWithAnUnfinishedHexNumber()
        {
            JSONString json = new ();
            Assert.False(json.Match(Quoted(@"a\u")).Success());
            Assert.False(json.Match(Quoted(@"a\u123")).Success());
        }
    }
}
