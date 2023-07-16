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

        [Test]
        public void Contractor_TopicOnly_ShouldGiveUsTheExpectedInstance()
        {
            var uri = new PublishMessageUri("my topic");

            uri.Exchange.Should().BeEmpty();
            uri.Topic.Should().Be("my topic");            
        }

        [TestCase("an-ex", "a-topic", "publish-message:a-topic@an-ex")]
        [TestCase("an ex", "a topic", "publish-message:a topic@an ex")]
        [TestCase("an.ex.2", "a.topic.2", "publish-message:a.topic.2@an.ex.2")]
        [TestCase("an.ex.2 $%@:/", "a.topic.2 $%@:/", "publish-message:a.topic.2 %24%25%40%3A%2F@an.ex.2 %24%25%40%3A%2F")]
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

            uri.ToString().Should().Be("publish-message:a-topic@an-ex?single=a-val&double=first&double=second");
        }

        [TestCase("an-ex", "a-topic", "publish-message:a-topic@an-ex")]
        [TestCase("an ex", "a topic", "publish-message:a topic@an ex")]
        [TestCase("an.ex.2", "a.topic.2", "publish-message:a.topic.2@an.ex.2")]
        [TestCase("an.ex.2 $%@:/", "a.topic.2 $%@:/", "publish-message:a.topic.2 %24%25%40%3A%2F@an.ex.2 %24%25%40%3A%2F")]
        public void ToUri_ShouldReturnTheExpectedResult(string exchange, string topic, string expectedResult)
        {
            new PublishMessageUri(exchange, topic).ToUri().Should().Be(new Uri(expectedResult));
        }        

        [TestCase("", false, "", "", "")]
        [TestCase("wrong-scheme:aqueue", false, "", "", "")]
        [TestCase("publish-message:too-many-segments/another", false, "", "", "")]
        [TestCase("publish-message://ahost/too-many-segments/another", false, "", "", "")]
        [TestCase("publish-message://ahost/aqueue", false, "", "", "")]
        [TestCase("publish-message:a-topic@an-ex", true, "an-ex", "a-topic", "")]
        [TestCase("publish-message:a-topic@an-ex", true, "an-ex", "a-topic", "")]
        [TestCase("publish-message:qwe@an-ex%2fasd", true, "an-ex/asd", "qwe", "")]
        [TestCase("publish-message:topic@an ex %24%25%40%3A%2F", true, "an ex $%@:/", "topic", "")]
        [TestCase("publish-message:topic@anex?param1=1&param2=2&param2=3", true, "anex", "topic", "param1=1&param2=2&param2=3")]
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

        [TestCase("wrong-scheme:aqueue", false, "", "", "")]
        [TestCase("publish-message:too-many-segments/another", false, "", "", "")]
        [TestCase("publish-message://ahost/too-many-segments/another", false, "", "", "")]
        [TestCase("publish-message://ahost/aqueue", false, "", "", "")]
        [TestCase("publish-message:a-topic@an-ex", true, "an-ex", "a-topic", "")]
        [TestCase("publish-message:a-topic@an-ex", true, "an-ex", "a-topic", "")]
        [TestCase("publish-message:qwe@an-ex%2fasd", true, "an-ex/asd", "qwe", "")]
        [TestCase("publish-message:qwe", true, "", "qwe", "")]
        [TestCase("publish-message:topic@an ex %24%25%40%3A%2F", true, "an ex $%@:/", "topic", "")]
        [TestCase("publish-message:topic@anex?param1=1&param2=2&param2=3", true, "anex", "topic", "param1=1&param2=2&param2=3")]
        public void TryParse_GivenAUri_ItShouldReturnTheExpectedResult(string uri, bool expectedResult, string expectedExchange, string expectedTopic, string expectedQueryParameters)
        {
            var succeeded = PublishMessageUri.TryParse(new Uri(uri), out var result);
            succeeded.Should().Be(expectedResult);

            if (expectedResult)
            {
                result.Exchange.Should().Be(expectedExchange);
                result.Topic.Should().Be(expectedTopic);
                result.QueryParameters.ToString().Should().Be(expectedQueryParameters);
            }
        }        

        [TestCase("publish-message:a-topic@asd@qwe", false, "", "", "")]
        [TestCase("publish-message:a-topic@asd", true, "a-topic", "asd", "a-topic@asd")]
        public void TryParse_GivenAQueueAndAnExchange_ItShouldReturnTheExpectedResult(string uri, bool expectedResult, string expectedTopic, string expectedExchange, string expectedTopicAndExchange)
        {
            var succeeded = PublishMessageUri.TryParse(uri, out var result);
            succeeded.Should().Be(expectedResult);

            if (expectedResult)
            {
                result.Topic.Should().Be(expectedTopic);
                result.Exchange.Should().Be(expectedExchange);
                result.TopicAndExchange.Should().Be(expectedTopicAndExchange);                
            }
        }
        
        [Test]
        public void Parse_GivenAValidString_ItShouldReturnAUri()
        {
            var result = PublishMessageUri.Parse("publish-message:topic@my-ex");
            
            result.Exchange.Should().Be("my-ex");
            result.Topic.Should().Be("topic");
        }

        [Test]
        public void Parse_GivenAValidUri_ItShouldReturnAUri()
        {
            var result = PublishMessageUri.Parse(new Uri("publish-message:topic@my-ex"));
            
            result.Exchange.Should().Be("my-ex");
            result.Topic.Should().Be("topic");
        }        

        [TestCase("", "Invalid URI format")]
        [TestCase("wrong-scheme:topic/exchange", "Expected a scheme of 'publish-message' but found 'wrong-scheme'")]
        [TestCase("publish-message:topic/exchange", "Expected to find one path segment but found 2 (topic/exchange)")]
        [TestCase("publish-message://ahost/topic/too-many-segments", "Expected to find no host name but found 'ahost'")]
        [TestCase("publish-message://ahost/topic", "Expected to find no host name but found 'ahost'")]
        public void Parse_GivenAnInvalidUri_ItShouldThrowAFormatException(string uri, string expectedMessage)
        {            
            new Action(() => PublishMessageUri.Parse(uri))
                .Should()
                .Throw<FormatException>()
                .WithMessage(expectedMessage);
        }
    }
}
