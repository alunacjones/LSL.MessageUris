using System;
using System.Web;

namespace LSL.MessageUris
{
    /// <summary>
    /// Uri to encapsulate a destination for message publishing as 'publish-message://{Exchange}/{Topic}?any=user&amp;defined=options'
    /// </summary>
    public class PublishMessageUri : UriWithOptionalQueryString
    {
        /// <summary>
        /// Constructor for setting up the topic and destination exchange
        /// </summary>
        /// <param name="exchange"></param>
        /// <param name="topic"></param>
        public PublishMessageUri(string exchange, string topic) 
        {
            Exchange = exchange;
            Topic = topic;
        }

        /// <summary>
        /// Constructor for only setting up a topic
        /// </summary>
        /// <param name="topic"></param>
        /// <returns></returns>
        public PublishMessageUri(string topic) : this(string.Empty, topic) {}

        /// <summary>
        /// The exchange name
        /// </summary>
        /// <value></value>
        public string Exchange { get; }

        /// <summary>
        /// The topic string
        /// </summary>
        /// <value></value>
        public string Topic { get; }

        /// <summary>
        /// If the exchange is set then it returns a string with the format `{Topic}@{DestinationExchange}` 
        /// otherwise it returns the the topic
        /// </summary>
        /// <value></value>
        public string TopicAndExchange => BuildFullName(IdentityFormatter);

        /// <summary>
        /// Renders the uri as a string of the format 'publish-message://{Exchange}/{Topic}?any=user&amp;defined=options'
        /// </summary>
        /// <returns></returns>
        public override string ToString() => ToUri().ToString();

        /// <summary>
        /// Returns the Uri representation of this PublishMessageUri instance
        /// </summary>
        /// <returns></returns>
        public Uri ToUri()
        {
            var builder = new UriBuilder();
            builder.Scheme = "publish-message";
            builder.Path = BuildFullName(Uri.EscapeDataString);
            builder.Host = string.Empty;
            builder.Query = QueryParameters.ToString();

            return builder.Uri;
        }

        /// <summary>
        /// Parses a string into a PublishMessageUri
        /// </summary>
        /// <param name="uri"></param>
        /// <returns>The parsed result</returns>
        /// <exception cref="System.FormatException">Gets thrown if the provide string was not in the correct format</exception>
        public static PublishMessageUri Parse(string uri)
        {
            var parseResult = InnerTryParse(uri, out var result);

            if (!parseResult.Success)
            {
                throw new FormatException(parseResult.Error);
            }

            return result;
        }

        /// <summary>
        /// Parses a Uri into a PublishMessageUri
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public static PublishMessageUri Parse(Uri uri) => Parse(uri.ToString());

        /// <summary>
        /// Tries to parse a Uri into a PublishMessageUri
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool TryParse(Uri uri, out PublishMessageUri result) => TryParse(uri.ToString(), out result);

        /// <summary>
        /// Tries to parse a string into a PublishMessageUri
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool TryParse(string uri, out PublishMessageUri result) => InnerTryParse(uri, out result).Success;

        private static (bool Success, string Error) InnerTryParse(string uri, out PublishMessageUri result)
        {
            if (!Uri.TryCreate(uri, UriKind.Absolute, out var realUri))
            {
                result = null;
                return (false, "Invalid URI format");
            }

            if (realUri.Scheme != "publish-message")
            {
                result = null;
                return (false, $"Expected a scheme of 'publish-message' but found '{realUri.Scheme}'");
            }

            if (realUri.Host != string.Empty)
            {
                result = null;
                return (false, $"Expected to find no host name but found '{realUri.Host}'");
            }

            if (realUri.Segments.Length != 1)
            {
                result = null;
                return (false, $"Expected to find one path segment but found {realUri.Segments.Length} ({realUri.LocalPath})");
            }

            var split = realUri.Segments[0].Split('@');

            if (split.Length > 2)
            {
                result = null;
                return (false, $"Expected to find one '@' synmbol but found {split.Length - 1} ({realUri.LocalPath})");                
            }

            var topic =  Uri.UnescapeDataString(split[0]);
            var exchange = split.Length == 2 
                ? Uri.UnescapeDataString(split[1])
                : string.Empty;

            result = new PublishMessageUri(exchange, topic);
            result.QueryParameters = HttpUtility.ParseQueryString(realUri.Query);

            return (true, string.Empty);
        }

        private string BuildFullName(Func<string, string> formatter) => BuildFullName(formatter, Topic, Exchange);
    }
}