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
        /// Default constructor
        /// </summary>
        /// <param name="exchange"></param>
        /// <param name="topic"></param>
        public PublishMessageUri(string exchange, string topic) 
        {
            Exchange = exchange;
            Topic = topic;
        }

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
            builder.Path = $"{Uri.EscapeDataString(Exchange)}/{Uri.EscapeDataString(Topic)}";
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

            if (realUri.Segments.Length != 2)
            {
                result = null;
                return (false, $"Expected to find two path segments but found {realUri.Segments.Length} ({realUri.LocalPath})");
            }

            result = new PublishMessageUri(
                Uri.UnescapeDataString(realUri.Segments[0].Trim('/')),
                Uri.UnescapeDataString(realUri.Segments[1].Trim('/'))
            );

            result.QueryParameters = HttpUtility.ParseQueryString(realUri.Query);

            return (true, string.Empty);
        }
    }
}