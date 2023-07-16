using System;
using System.Web;

namespace LSL.MessageUris
{
    /// <summary>
    /// Uri to encapsulate a destination for message sending as 'send-message://{DestinationQueue}?any=user&amp;defined=options'
    /// </summary>
    public class SendMessageUri : UriWithOptionalQueryString
    {
        /// <summary>
        /// Constrcutor to use when only specifying a queue (exchange will be an empty string)
        /// </summary>
        /// <param name="destinationQueue">Initalises the DestinationQueue value to this</param>
        public SendMessageUri(string destinationQueue) : this(destinationQueue, string.Empty) 
        {
            DestinationQueue = destinationQueue;            
        }

        /// <summary>
        /// Constructor to use when needing to encode a queue and an exchange
        /// </summary>
        /// <param name="destinationQueue"></param>
        /// <param name="destinationExchange"></param>
        public SendMessageUri(string destinationQueue, string destinationExchange)
        {
            DestinationQueue = destinationQueue;
            DestinationExchange = destinationExchange;
        }

        /// <summary>
        /// The destination queue name
        /// </summary>
        /// <value></value>
        public string DestinationQueue { get; }

        /// <summary>
        /// The destination exchange name
        /// </summary>
        /// <value></value>
        public string DestinationExchange { get; }

        /// <summary>
        /// If the exchange is set the it returns a string with the format `{DestinationQueue}@{DestinationExchange}
        /// otherwise it returns the destination queue
        /// </summary>
        /// <value></value>
        public string DestinationQueueAndExchange => BuidFullName(IdentityFormatter);

        /// <summary>
        /// Renders the uri as a string of the format 'send-message://{DestinationQueue}?any=user&amp;defined=options'
        /// </summary>
        /// <returns></returns>
        public override string ToString() => ToUri().ToString();

        /// <summary>
        /// Returns the Uri representation of this SendMessageUri instance
        /// </summary>
        /// <returns></returns>
        public Uri ToUri()
        {
            var builder = new UriBuilder();
            builder.Scheme = "send-message";
            builder.Path = BuidFullName(Uri.EscapeDataString);
            builder.Host = string.Empty;
            builder.Query = QueryParameters.ToString();

            return builder.Uri;
        }

        /// <summary>
        /// Parses a string into a SendMessageUri
        /// </summary>
        /// <param name="uri"></param>
        /// <returns>The parsed result</returns>
        /// <exception cref="System.FormatException">Gets thrown if the provide string was not in the correct format</exception>
        public static SendMessageUri Parse(string uri)
        {
            var parseResult = InnerTryParse(uri, out var result);

            if (!parseResult.Success)
            {
                throw new FormatException(parseResult.Error);
            }

            return result;
        }

        /// <summary>
        /// Tries to parse a string into a SendMessageUri
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool TryParse(string uri, out SendMessageUri result) => InnerTryParse(uri, out result).Success;

        private static (bool Success, string Error) InnerTryParse(string uri, out SendMessageUri result)
        {
            if (!Uri.TryCreate(uri, UriKind.Absolute, out var realUri))
            {
                result = null;
                return (false, "Invalid URI format");
            }

            if (realUri.Scheme != "send-message")
            {
                result = null;
                return (false, $"Expected a scheme of 'send-message' but found '{realUri.Scheme}'");
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

            var queue =  Uri.UnescapeDataString(split[0]);
            var exchange = split.Length == 2 
                ? Uri.EscapeDataString(split[1])
                : string.Empty;

            result = new SendMessageUri(queue, exchange);
            result.QueryParameters = HttpUtility.ParseQueryString(realUri.Query);

            return (true, string.Empty);
        }

        private static Func<string, string> IdentityFormatter = new Func<string, string>(s => s);

        private string BuidFullName(Func<string, string> formatter) =>
            string.IsNullOrEmpty(DestinationExchange)
                ? formatter(DestinationQueue)
                : $"{formatter(DestinationQueue)}@{formatter(DestinationExchange)}";
    }
}