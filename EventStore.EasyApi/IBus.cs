using System;
using EventStore.ClientAPI;

namespace EventStore.EasyApi
{
    public interface IBus : IDisposable
    {
        void Publish<T>(T message) where T : class;
        void Publish<T>(T message, string exchangeName) where T : class;
        void Subscribe<T>(string subscriptionId, Action<dynamic> onMessage) where T : class;
        void Subscribe<T>(string subscriptionId, Action<dynamic> onMessage, Action<ISubscriptionConfiguration> configure)
            where T : class;
        void SubscribeAsync<T>(string subscriptionId, Action<dynamic> onMessage,
            Action<ISubscriptionConfiguration> configure) where T : class;
        bool IsConnected { get; }
    }
}
