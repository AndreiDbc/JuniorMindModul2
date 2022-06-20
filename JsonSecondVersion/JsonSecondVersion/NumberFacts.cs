using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace JsonSecondVersion
{
    public class NumberFacts
    {
        [Fact]
        public void CanBeZero()
        {
            var number = new Number();
            Assert.True(number.Match("0").Success());
        }

        [Fact]
        public void DoesNotContainLetters()
        {
            var number = new Number();
            Assert.False(number.Match("a").Success());
        }

        [Fact]
        public void CanHaveASingleDigit()
        {
            var number = new Number();
            Assert.True(number.Match("7").Success());
        }

        [Fact]
        public void CanHaveMultipleDigits()
        {
            var number = new Number();
            Assert.True(number.Match("70").Success());
        }

        [Fact]
        public void IsNotAnEmptyString()
        {
            var number = new Number();
            Assert.False(number.Match(string.Empty).Success());
        }

        [Fact]
        public void DoesNotStartWithZero()
        {
            var number = new Number();
            Assert.True(number.Match("07").Success());
        }

        [Fact]
        public void CanBeNegative()
        {
            var number = new Number();
            Assert.True(number.Match("-26").Success());
        }

        [Fact]
        public void CanBeMinusZero()
        {
            var number = new Number();
            Assert.True(number.Match("-0").Success());
        }

        [Fact]
        public void CanBeFractional()
        {
            var number = new Number();
            Assert.True(number.Match("12.34").Success());
        }

        [Fact]
        public void CanBeFractionalShouldReturnEmptyString()
        {
            var number = new Number();
            Assert.Equal("", number.Match("12.34").RemainingText());
        }

        [Fact]
        public void TheFractionCanHaveLeadingZeros()
        {
            var number = new Number();
            Assert.True(number.Match("0.00000001").Success());
            Assert.True(number.Match("10.00000001").Success());
        }

        [Fact]
        public void DoesNotHaveTwoFractionParts()
        {
            var number = new Number();
            Assert.True(number.Match("12.34.56").Success());
        }

        [Fact]
        public void DoesNotHaveTwoFractionPartsShouldReturnRemainingText()
        {
            var number = new Number();
            Assert.Equal(".56", number.Match("12.34.56").RemainingText());
        }

        [Fact]
        public void TheDecimalPartDoesNotAllowLetters()
        {
            var number = new Number();
            Assert.True(number.Match("12.3x").Success());
        }

        [Fact]
        public void TheDecimalPartDoesNotAllowLettersShouldReturnRemainingText()
        {
            var number = new Number();
            Assert.Equal("x", number.Match("12.3x").RemainingText());
        }

        [Fact]
        public void CanHaveAnExponent()
        {
            var number = new Number();
            Assert.True(number.Match("12e3").Success());
        }

        [Fact]
        public void TheExponentCanStartWithCapitalE()
        {
            var number = new Number();
            Assert.True(number.Match("12E3").Success());
        }

        [Fact]
        public void TheExponentCanStartWithCapitalEShoudlReturnEmptyString()
        {
            var number = new Number();
            Assert.Equal("", number.Match("12E3").RemainingText());
        }

        [Fact]
        public void TheExponentCanHavePositive()
        {
            var number = new Number();
            Assert.True(number.Match("12e+3").Success());
        }

        [Fact]
        public void TheExponentCanBeNegative()
        {
            var number = new Number();
            Assert.True(number.Match("61e-9").Success());
        }

        [Fact]
        public void CanHaveFractionAndExponent()
        {
            var number = new Number();
            Assert.True(number.Match("12.34E3").Success());
        }

        [Fact]
        public void CanHaveFractionAndExponentShouldReturnEMptyString()
        {
            var number = new Number();
            Assert.Equal("", number.Match("12.34E3").RemainingText());
        }

        [Fact]
        public void TheExponentDoesNotAllowLetters()
        {
            var number = new Number();
            Assert.True(number.Match("22e3x3").Success());
        }

        [Fact]
        public void TheExponentDoesNotAllowLettersShoudlReturnRemainingText()
        {
            var number = new Number();
            Assert.Equal("x3", number.Match("22e3x3").RemainingText());
        }

        [Fact]
        public void DoesNotHaveTwoExponents()
        {
            var number = new Number();
            Assert.True(number.Match("22e323e33").Success());
        }

        [Fact]
        public void DoesNotHaveTwoExponentsShouldReturnRemainingText()
        {
            var number = new Number();
            Assert.Equal("e33", number.Match("22e323e33").RemainingText());
        }

        [Fact]
        public void TheExponentIsBeforeTheFraction()
        {
            var number = new Number();
            Assert.True(number.Match("22e3.3").Success());
        }

        [Fact]
        public void TheExponentIsBeforeTheFractionShouldReturnRemainingText()
        {
            var number = new Number();
            Assert.Equal(".3", number.Match("22e3.3").RemainingText());
        }
    }
}
