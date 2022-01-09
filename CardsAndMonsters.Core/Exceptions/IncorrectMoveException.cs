using System;
using System.Runtime.Serialization;

namespace CardsAndMonsters.Core.Exceptions
{
    [Serializable]
    public class IncorrectMoveException : Exception
    {
        private static readonly string DefaultMessage = "Invalid move attempted.";

        public IncorrectMoveException() : base(DefaultMessage) { }
        public IncorrectMoveException(string message) : base(message) { }
        public IncorrectMoveException(string message, Exception innerException)
        : base(message, innerException) { }

        protected IncorrectMoveException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
