using System.Collections.Generic;

namespace EventStore.EasyApi.Sprache
{
    public interface IFailure<out T> : IResult<T>
    {
        string Message { get; }
        IEnumerable<string> Expectations { get; }
        Input FailedInput { get; }
    }
}