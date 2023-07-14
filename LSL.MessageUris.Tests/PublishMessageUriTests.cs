using System;
using FluentAssertions;
using NUnit.Framework;

namespace LSL.MessageUris.Tests
{
    public class PublishUriTests
    {
        [Test]
        public void Constructor_ShouldGiveUsTheExpectedInstance()
        {
            var uri = new PublishMessageUri("my exchange", "my topic");

            uri.Exchange.Should().Be("my exchange");
            uri.Topic.Should().Be("my topic");
        }

        [TestCase("an-ex", "a-topic", "publish-message:an-ex/a-topic")]
        [TestCase("an ex", "a topic", "publish-message:an ex/a topic")]
        [TestCase("an.ex.2", "a.topic.2", "publish-message:an.ex.2/a.topic.2")]
        [TestCase("an.ex.2 $%@:/", "a.topic.2 $%@:/", "publish-message:an.ex.2 %24%25%40%3A%2F/a.topic.2 %24%25%40%3A%2F")]
        public void ToString_ShouldReturnTheExpectedResult(string exchange, string topic, string expectedResult)
        {
            new PublishMessageUri(exchange, topic).ToString().Should().Be(expectedResult);
        }

        [Test]
        public void ToString_WithQueryParameters_ShouldReturnTheExpectedResult()
        {
            var uri = new PublishMessageUri("an-ex", "a-topic")
                .AddQueryParameter("single", "a-val")
                .AddQueryParameter("double", "first")
                .AddQueryParameter("double", "second");

            uri.ToString().Should().Be("publish-message:an-ex/a-topic?single=a-val&double=first&double=second");
        }

        [TestCase("an-ex", "a-topic", "publish-message:an-ex/a-topic")]
        [TestCase("an ex", "a topic", "publish-message:an ex/a topic")]
        [TestCase("an.ex.2", "a.topic.2", "publish-message:an.ex.2/a.topic.2")]
        [TestCase("an.ex.2 $%@:/", "a.topic.2 $%@:/", "publish-message:an.ex.2 %24%25%40%3A%2F/a.topic.2 %24%25%40%3A%2F")]
        public void ToUri_ShouldReturnTheExpectedResult(string exchange, string topic, string expectedResult)
        {
            new PublishMessageUri(exchange, topic).ToUri().Should().Be(new Uri(expectedResult));
        }        

        [TestCase("", false, "", "", "")]
        [TestCase("wrong-scheme:aqueue", false, "", "", "")]
        [TestCase("publish-message:too-few-segments", false, "", "", "")]
        [TestCase("publish-message://ahost/too-few-segments", false, "", "", "")]
        [TestCase("publish-message://ahost/aqueue/qwe", false, "", "", "")]
        [TestCase("publish-message:an-ex/a-topic", true, "an-ex", "a-topic", "")]
        [TestCase("publish-message:an-ex%2fasd/qwe", true, "an-ex/asd", "qwe", "")]
        [TestCase("publish-message:an ex %24%25%40%3A%2F/topic", true, "an ex $%@:/", "topic", "")]
        [TestCase("publish-message:anex/topic?param1=1&param2=2&param2=3", true, "anex", "topic", "param1=1&param2=2&param2=3")]
        public void TryParse_GivenAString_ItShouldReturnTheExpectedResult(string uri, bool expectedResult, string expectedExchange, string expectedTopic, string expectedQueryParameters)
        {
            var succeeded = PublishMessageUri.TryParse(uri, out var result);
            succeeded.Should().Be(expectedResult);

            if (expectedResult)
            {
                result.Exchange.Should().Be(expectedExchange);
                result.Topic.Should().Be(expectedTopic);
                result.QueryParameters.ToString().Should().Be(expectedQueryParameters);
            }
        }

        [Test]
        public void Parse_GivenAValidString_ItShouldReturnAUri()
        {
            var result = PublishMessageUri.Parse("publish-message:my-ex/topic");
            
            result.Exchange.Should().Be("my-ex");
            result.Topic.Should().Be("topic");
        }

        [TestCase("", "Invalid URI format")]
        [TestCase("wrong-scheme:exchange/topic", "Expected a scheme of 'publish-message' but found 'wrong-scheme'")]
        [TestCase("publish-message:exchange/too-many-segments/another", "Expected to find two path segments but found 3 (exchange/too-many-segments/another)")]
        [TestCase("publish-message:exchange", "Expected to find two path segments but found 1 (exchange)")]
        [TestCase("publish-message://ahost/exchange/too-many-segments", "Expected to find no host name but found 'ahost'")]
        [TestCase("publish-message://ahost/exchange", "Expected to find no host name but found 'ahost'")]
        public void Parse_GivenAnInvalidUri_ItShouldThrowAFormatException(string uri, string expectedMessage)
        {
            new Action(() => PublishMessageUri.Parse(uri))
                .Should()
                .Throw<FormatException>()
                .WithMessage(expectedMessage);
        }
    }
}
