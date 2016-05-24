using System;
using System.Runtime.Serialization;

namespace EventStore.EasyApi
{
    [Serializable]
    public class ClientException : Exception
    {
        public ClientException() { }
        public ClientException(string message) : base(message) { }
        public ClientException(string format, params string[] args) : base(string.Format(format, args)) { }
        public ClientException(string message, Exception inner) : base(message, inner) { }

        protected ClientException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        { }
    }

    [Serializable]
    public class ClientInvalidMessageTypeException : ClientException
    {
        public ClientInvalidMessageTypeException() { }
        public ClientInvalidMessageTypeException(string message) : base(message) { }
        public ClientInvalidMessageTypeException(string format, params string[] args) : base(format, args) { }
        public ClientInvalidMessageTypeException(string message, Exception inner) : base(message, inner) { }
        protected ClientInvalidMessageTypeException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }

    [Serializable]
    public class ClientResponderException : ClientException
    {
        public ClientResponderException() { }
        public ClientResponderException(string message) : base(message) { }
        public ClientResponderException(string format, params string[] args) : base(format, args) { }
        public ClientResponderException(string message, Exception inner) : base(message, inner) { }
        protected ClientResponderException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
