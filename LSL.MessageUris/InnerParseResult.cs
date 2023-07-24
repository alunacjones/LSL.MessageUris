using System.Collections.Generic;

namespace LSL.MessageUris
{
    internal struct InnerParseResult
    {
        public bool Success { get; private set; }
        public string Error { get; private set; }

        public InnerParseResult(bool success, string error)
        {
            Success = success;
            Error = error;
        }

        public override bool Equals(object obj)
        {
            return obj is InnerParseResult other &&
                   Success == other.Success &&
                   Error == other.Error;
        }

        public override int GetHashCode()
        {
            int hashCode = -1489853407;
            hashCode = hashCode * -1521134295 + Success.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Error);
            return hashCode;
        }
    }
}