using System;
using System.Runtime.Serialization;

namespace CardsAndMonsters.Core.Exceptions
{
    [Serializable]
    public class GameArgumentException<T> : Exception where T : class
    {

        private static readonly string DefaultMessage = "Unexpected argument provided.";

        public string ArgumentName { get; set; }
        public Type ArgumentType { get; set; }
#nullable enable
        public object? ArgumentProvided { get; set; }

        public GameArgumentException() : base(DefaultMessage) { }
        public GameArgumentException(string message) : base(message) { }
        public GameArgumentException(string message, Exception innerException)
        : base(message, innerException) { }

        public GameArgumentException(string argumentName, object? argumentProvided)
        : base(DefaultMessage)
        {
            ArgumentName = argumentName;
            ArgumentType = typeof(T);
            ArgumentProvided = argumentProvided;
        }

        public GameArgumentException(string argumentName, object? argumentProvided, Exception innerException)
        : base(DefaultMessage, innerException)
        {
            ArgumentName = argumentName;
            ArgumentType = typeof(T);
            ArgumentProvided = argumentProvided;
        }
#nullable disable
        protected GameArgumentException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }

}
