using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OpenDnD.Interfaces
{
    public class InvalidAuthenticationTokenException : Exception
    {
        public const string DefaultMessage = "Invalid Authentication Token";
        public InvalidAuthenticationTokenException() : base(DefaultMessage)
        {
        }

        public InvalidAuthenticationTokenException(string? message) : base(message ?? DefaultMessage)
        {
        }

        public InvalidAuthenticationTokenException(string? message, Exception? innerException) : base(message ?? DefaultMessage, innerException)
        {
        }

        protected InvalidAuthenticationTokenException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
    public class NoAccessToActionException : Exception
    {
        public const string DefaultMessage = "No Access to this acction";
        public NoAccessToActionException() : base(DefaultMessage)
        {
        }

        public NoAccessToActionException(string? message) : base(message ?? DefaultMessage)
        {
        }

        public NoAccessToActionException(string? message, Exception? innerException) : base(message ?? DefaultMessage, innerException)
        {
        }

        protected NoAccessToActionException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

    public class NoEntryWithRequiredIdException<T> : Exception
    {
        public const string DefaultFormatMessage = "No entry of type {0} with id {1} exist";
        public NoEntryWithRequiredIdException(Guid id) : base (string.Format(DefaultFormatMessage, typeof(T).Name, id))
        {
        }

        public NoEntryWithRequiredIdException(Guid id, Exception? innerException) : base(string.Format(DefaultFormatMessage, typeof(T).Name, id), innerException)
        {
        }

        protected NoEntryWithRequiredIdException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
