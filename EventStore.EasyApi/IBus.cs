using System;
using System.Threading.Tasks;
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

    public interface IEasyBus : IDisposable
    {
        void Publish(object message);
        void Subscribe(string subscriptionId, Action<EventStoreCatchUpSubscription> action);
        bool IsConnected { get; }
        // a single stream with a max count set to one
        Task<WriteResult> SavepointAsync(string identifier, Position position);
    }
}
