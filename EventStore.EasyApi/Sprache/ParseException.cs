using System;

namespace EventStore.EasyApi.Sprache
{
    public class ParseException : Exception
    {
        public ParseException(string message)
            : base(message)
        {
        }
    }
}
