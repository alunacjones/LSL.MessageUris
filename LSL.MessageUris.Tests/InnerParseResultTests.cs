using System;
using FluentAssertions;
using NUnit.Framework;

namespace LSL.MessageUris.Tests
{
    public class InnerParseResultTests
    {
        [Test]
        public void Equals_GivenAResultAndItIsComparedToAnotherInstanceThatIsSetupTheSame_ThenItShouldReturnTrue()
        {
            new InnerParseResult(true, string.Empty)
             .Equals(new InnerParseResult(true, string.Empty))
             .Should()
             .BeTrue();
        }

        [TestCase(false, "my-string")]
        [TestCase(true, "my-string")]
        public void Equals_GivenAResultAndItIsComparedToAnotherInstanceThatIsSetupDifferently_ThenItShouldReturnFalse(bool success, string error)
        {
            new InnerParseResult(true, string.Empty)
             .Equals(new InnerParseResult(success, error))
             .Should()
             .BeFalse();
        }

        [Test]
        public void Equals_GivenAResultAndItIsComparedToNull_ThenItShouldReturnFalse()
        {
            new InnerParseResult(true, string.Empty)
                .Equals(null)
                .Should()
                .BeFalse();
        }

        [Test]
        public void GetHashCode_GivenAResult_ItShouldProduceTheExpectedHashCode()
        {
            new Action(() => new InnerParseResult(true, string.Empty).GetHashCode())
                .Should()
                .NotThrow();
        }
    }
}
