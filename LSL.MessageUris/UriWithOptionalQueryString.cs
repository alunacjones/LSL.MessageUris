using System.Collections.Specialized;
using System.Web;

namespace LSL.MessageUris
{
    /// <summary>
    /// Abstract base class that allows a custom Uri to inherit from and contain an optional query string
    /// </summary>
    public abstract class UriWithOptionalQueryString
    {
        /// <summary>
        /// The query parameters
        /// </summary>
        /// <returns></returns>
        public NameValueCollection QueryParameters { get; protected set; } = HttpUtility.ParseQueryString(string.Empty);
    }
}