using System;
using System.Configuration;
using EventStore.EasyApi.ConnectionString;

namespace EventStore.EasyApi
{
    public static class EasyClient
    {
        public static IBus CreateBus()
        {
            return CreateBus(c => { });
        }

        public static IBus CreateBus(Action<IServiceRegister> registerServices)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["EventStore"];
            if (connectionString == null)
            {
                throw new ClientException(
                    "Could not find a connection string for an Event Store. " +
                    "Please add a connection string in the <ConnectionStrings> section" +
                    "of the application's configuration file. For example: " +
                    "<add name=\"EventStore\" connectionString=\"host=localhost\" />");
            }

            return CreateBus(connectionString.ConnectionString, registerServices);
        }
        
        public static IBus CreateBus(string connectionString, Action<IServiceRegister> registerServices)
        {
            Preconditions.CheckNotNull(connectionString, "connectionString");
            Preconditions.CheckNotNull(registerServices, "registerServices");

            var connectionStringParser = new ConnectionStringParser();

            var connectionConfiguration = connectionStringParser.Parse(connectionString);

            return CreateBus(connectionConfiguration, registerServices);
        }
        
        public static IBus CreateBus(ConnectionConfiguration connectionConfiguration, Action<IServiceRegister> registerServices)
        {
            Preconditions.CheckNotNull(connectionConfiguration, "connectionConfiguration");
            Preconditions.CheckNotNull(registerServices, "registerServices");

            return new EventStoreBus(connectionConfiguration);
        }
    }
}
