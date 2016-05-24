using System;
using System.Net;
using System.Web.Script.Serialization;
using EventStore.ClientAPI;
using EventStore.ClientAPI.SystemData;
using EventStore.EasyApi.Internals;

namespace EventStore.EasyApi
{
    public class EventStoreBus : IBus
    {
        private readonly ConnectionConfiguration _connectionConfiguration;

        public EventStoreBus(ConnectionConfiguration connectionConfiguration)
        {
            _connectionConfiguration = connectionConfiguration;
        }

        public void Dispose()
        {
            _eventStoreConnection?.Close();
        }

        public void Publish<T>(T message) where T : class
        {
            Preconditions.CheckNotNull(message, "message");

            Publish(message, typeof(T).ToString());
        }

        public void Publish<T>(T message, string exchangeName) where T : class
        {
            Preconditions.CheckNotNull(message, "message");
            Preconditions.CheckNotNull(exchangeName, "topic");

            throw new NotImplementedException();
            //var connection = new Connection(new Address(_connectionConfiguration.ConnectionString.ToString()));
            //var session = new Session(connection);
            //var sender = new SenderLink(session, "request-client-sender", "request_processor");

            //var json = new JavaScriptSerializer().Serialize(message);
            //var message = new Message(json)
            //{
            //    Properties = new Properties()
            //    {
            //        MessageId = Guid.NewGuid().ToString(),
            //        ReplyTo = "client-" + Guid.NewGuid(),
            //        CorrelationId = exchangeName
            //    },
            //    ApplicationProperties = new ApplicationProperties()
            //};
            //sender.Send(message, null, null);
            //Console.WriteLine("Sent request {0} body {1}", message.Properties, message.Body);
        }

        public void Subscribe<T>(string subscriptionId, Action<dynamic> onMessage) where T : class
        {
            Subscribe<T>(subscriptionId, onMessage, x => { });
        }

        public void Subscribe<T>(string subscriptionId, Action<dynamic> onMessage, Action<ISubscriptionConfiguration> configure) where T : class
        {
            Preconditions.CheckNotNull(subscriptionId, "subscriptionId");
            Preconditions.CheckNotNull(onMessage, "onMessage");
            Preconditions.CheckNotNull(configure, "configure");

            SubscribeAsync<T>(subscriptionId, msg => TaskHelpers.ExecuteSynchronously(() => onMessage(msg)), configure);
        }

        private IEventStoreConnection _eventStoreConnection;

        public virtual void SubscribeAsync<T>(string subscriptionId, Action<dynamic> onMessage, Action<ISubscriptionConfiguration> configure) where T : class
        {
            Preconditions.CheckNotNull(subscriptionId, "subscriptionId");
            Preconditions.CheckNotNull(onMessage, "onMessage");
            Preconditions.CheckNotNull(configure, "configure");

            var exchangeName = typeof(T).ToString();
            //var serializer = new JavaScriptSerializer();
            _eventStoreConnection = EventStoreConnection.Create(new IPEndPoint(IPAddress.Loopback, 1113));
            //_eventStoreConnection.Connected +=
            //    (sender, args) => _eventStoreConnection.SubscribeToStreamAsync(exchangeName, false, (arg1, arg2) =>
            //    {
            //        var jsonEvent = serializer.Deserialize<dynamic>(arg2.Event.DebugDataView);
            //        var jsonMessageBody = jsonEvent["bodySection"]["value"];
            //        var msg = serializer.Deserialize<T>(jsonMessageBody);
            //        onMessage(msg);
            //    });

            CreateSubscription(_eventStoreConnection, subscriptionId, "EventStore.EasyApi");

            _eventStoreConnection.Connected +=
                (sender, args) => _eventStoreConnection.ConnectToPersistentSubscriptionAsync(exchangeName, "EventStore.EasyApi", (arg1, arg2) =>
                {
                    onMessage(arg2);
                });

            _eventStoreConnection.ConnectAsync().Wait();
        }

        private static void CreateSubscription(IEventStoreConnection conn, string stream, string group)
        {
            PersistentSubscriptionSettings settings = PersistentSubscriptionSettings.Create()
                .DoNotResolveLinkTos()
                .StartFromCurrent();

            try
            {
                conn.CreatePersistentSubscriptionAsync(stream, group, settings, new UserCredentials("admin", "changeit")).Wait();
            }
            catch (AggregateException ex)
            {
                if (ex.InnerException.GetType() != typeof(InvalidOperationException)
                    && ex.InnerException?.Message != $"Subscription group {group} on stream {stream} already exists")
                {
                    throw;
                }
            }
        }

        public bool IsConnected => _eventStoreConnection != null;
    }
}
