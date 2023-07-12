namespace LSL.MessageUris
{
    /// <summary>
    /// UriWithOptionalQueryStringExtensions
    /// </summary>
    public static class UriWithOptionalQueryStringExtensions
    {
        /// <summary>
        /// Fluently add query parameters
        /// </summary>
        /// <param name="source">the source instance to add the parameter to</param>
        /// <param name="key">The key name to add</param>
        /// <param name="value">The value to add</param>
        /// <typeparam name="T">Inferred</typeparam>
        /// <returns>The original instance of the uri</returns>
        public static T AddQueryParameter<T>(this T source, string key, string value) where T : UriWithOptionalQueryString
        {
            source.QueryParameters.Add(key, value);
            return source;
        }
    }
}