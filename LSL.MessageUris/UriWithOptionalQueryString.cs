using System;
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

        /// <summary>
        /// Build an optional @ separated string from the given parameters
        /// </summary>
        /// <param name="formatter">The format function</param>
        /// <param name="requiredPart">The part that will always be rendered</param>
        /// <param name="optionalPart">The optional part, if included will be appended to the required part with '@optionalPart'</param>
        /// <returns></returns>
        protected string BuildFullName(Func<string, string> formatter, string requiredPart, string optionalPart) =>
            string.IsNullOrEmpty(optionalPart)
                ? formatter(requiredPart)
                : $"{formatter(requiredPart)}@{formatter(optionalPart)}";        

        /// <summary>
        /// An identity function that returns the string it was given
        /// </summary>
        /// <returns></returns>
        protected static Func<string, string> IdentityFormatter = new Func<string, string>(s => s);                
    }
}