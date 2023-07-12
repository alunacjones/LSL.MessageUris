using System;
using FluentAssertions;
using NUnit.Framework;

namespace LSL.MessageUris.Tests
{
    public class SendMessageUriTests
    {
        [Test]
        public void Constructor_ShouldGiveUsTheExpectedInstance()
        {
            new SendMessageUri("my queue").DestinationQueue.Should().Be("my queue");
        }

        [TestCase("aqueue", "send-message:aqueue")]
        [TestCase("a queue", "send-message:a queue")]
        [TestCase("a.queue.again", "send-message:a.queue.again")]
        [TestCase("a queue $%@:/", "send-message:a queue %24%25%40%3A%2F")]
        public void ToString_ShouldReturnTheExpectedResult(string destinationQueue, string expectedResult)
        {
            new SendMessageUri(destinationQueue).ToString().Should().Be(expectedResult);
        }

        [Test]
        public void ToString_WithQueryParameters_ShouldReturnTheExpectedResult()
        {
            var uri = new SendMessageUri("aqueue")
                .AddQueryParameter("single", "a-val")
                .AddQueryParameter("double", "first")
                .AddQueryParameter("double", "second");

            uri.ToString().Should().Be("send-message:aqueue?single=a-val&double=first&double=second");
        }

        [TestCase("aqueue", "send-message:aqueue")]
        [TestCase("a queue", "send-message:a queue")]
        [TestCase("a.queue.again", "send-message:a.queue.again")]
        [TestCase("a queue $%@:/", "send-message:a queue %24%25%40%3A%2F")]
        public void ToUri_ShouldReturnTheExpectedResult(string destinationQueue, string expectedResult)
        {
            new SendMessageUri(destinationQueue).ToUri().Should().Be(new Uri(expectedResult));
        }        

        [TestCase("", false, "", "")]
        [TestCase("wrong-scheme:aqueue", false, "", "")]
        [TestCase("send-message:aqueue/too-many-segments", false, "", "")]
        [TestCase("send-message://ahost/aqueue/too-many-segments", false, "", "")]
        [TestCase("send-message://ahost/aqueue", false, "", "")]
        [TestCase("send-message:aqueue", true, "aqueue", "")]
        [TestCase("send-message:aqueue%2fasd", true, "aqueue/asd", "")]
        [TestCase("send-message:a queue %24%25%40%3A%2F", true, "a queue $%@:/", "")]
        [TestCase("send-message:aqueue?param1=1&param2=2&param2=3", true, "aqueue", "param1=1&param2=2&param2=3")]
        public void TryParse_GivenAString_ItShouldReturnTheExpectedResult(string uri, bool expectedResult, string expectedDestinationQueue, string expectedQueryParameters)
        {
            var succeeded = SendMessageUri.TryParse(uri, out var result);
            succeeded.Should().Be(expectedResult);

            if (expectedResult)
            {
                result.DestinationQueue.Should().Be(expectedDestinationQueue);
                result.QueryParameters.ToString().Should().Be(expectedQueryParameters);
            }
        }

        [Test]
        public void Parse_GivenAValidString_ItShouldReturnAUri()
        {
            SendMessageUri.Parse("send-message:my-queue").DestinationQueue.Should().Be("my-queue");
        }

        [Test]
        public void Test()
        {
            //TODO: Fix typo in base class for QueryParameters!!
            //ALSO: Test that encoding and decoding looks fine after uri conversion i.e. is the use of Segments okay to get unencoded parts?
            var uri = SendMessageUri.Parse("send-message:my-queue?my=12");
        }
        [TestCase("", "Invalid URI format")]
        [TestCase("wrong-scheme:aqueue", "Expected a scheme of 'send-message' but found 'wrong-scheme'")]
        [TestCase("send-message:aqueue/too-many-segments", "Expected to find one path segment but found 2 (aqueue/too-many-segments)")]
        [TestCase("send-message://ahost/aqueue/too-many-segments", "Expected to find no host name but found 'ahost'")]
        [TestCase("send-message://ahost/aqueue", "Expected to find no host name but found 'ahost'")]
        public void Parse_GivenAnInvalidUri_ItShouldThrowAFormatException(string uri, string expectedMessage)
        {
            new Action(() => SendMessageUri.Parse(uri))
                .Should()
                .Throw<FormatException>()
                .WithMessage(expectedMessage);
        }
    }
}
