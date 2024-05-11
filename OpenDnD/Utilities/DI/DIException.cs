using System.Runtime.Serialization;

namespace OpenDnD.Utilities.DI
{
    public class DIException : Exception
    {
        public DIException()
        {
        }

        public DIException(string? message) : base(message)
        {
        }

        public DIException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected DIException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

}
