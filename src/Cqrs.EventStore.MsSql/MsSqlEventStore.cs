using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using Cqrs.Commanding;
using Cqrs.EventStore.MsSql.FluentSqlClient;
using Cqrs.Eventing;

namespace Cqrs.EventStore.MsSql
{
    public class MsSqlEventStore : SerializedEventStore<EventDescriptor>
    {

        private const string ConnectionStringName = "MsSqlEventStore";

        public MsSqlEventStore(
            IEventPublisher publisher, 
            ICommandSender commandSender, 
            ISerializer<EventDescriptor> serializer) 
            : base(publisher, commandSender, serializer)
        {
        }

        private string GetConnectionString()
        {
            return ConfigurationManager
                .ConnectionStrings[ConnectionStringName]
                .ConnectionString;
        }

        protected override IEnumerable<EventDescriptor> LoadSerializedEvents(Guid eventSourceId)
        {

            var eventSourceVersion = 0;
            var eventDescriptors = new List<EventDescriptor>();

            Action<int> getEventSourceVersion = i => eventSourceVersion = i;

            Action<IDataReader> forEachRow =
                rdr =>
                    {
                        var id = eventSourceId;
                        var version = rdr.GetInt32(0);
                        var typeName = rdr.GetString(1);
                        var data = rdr.GetString(2);

                        if (version > eventSourceVersion)
                            throw new ConcurrencyException();

                        eventDescriptors.Add(
                            new EventDescriptor(data, typeName, id, version));
                    };

            Tx.With(GetConnectionString())
                .Scalar<int>(
                    Queries.SelectVersion,
                    new Dictionary<string, object>() {{"id", eventSourceId}},
                    getEventSourceVersion)
                .Reader(
                    Queries.SelectAllEvents,
                    new Dictionary<string, object>() {{"id", eventSourceId}},
                    forEachRow)
                .Execute();

            return eventDescriptors;
        }

        protected override void PersistSerializedEvents(IEnumerable<EventDescriptor> serializedEvents, Guid eventSourceId, int expectedVersion)
        {
            var events = serializedEvents.ToArray();
            var newVersion = events.Max(ed => ed.Version);

            var tx = Tx.With(GetConnectionString());
            IExecutableSyntax sqlBatch;

            sqlBatch = expectedVersion <= 0
                           ? InsertEventSource(tx, eventSourceId, newVersion)
                           : UpdateVersion(tx, eventSourceId, expectedVersion, newVersion);

            sqlBatch = InsertEvents(sqlBatch, eventSourceId, events);

            sqlBatch.Execute();
        }

        private IExecutableSyntax InsertEventSource(ITransactionSyntax tx, Guid eventSourceId, int newVersion)
        {
            return tx.NonQuery(
                Queries.InsertEventSource,
                new Dictionary<string, object>()
                    {
                        {"id", eventSourceId},
                        {"newVersion", newVersion}
                    },
                rowsAffected =>
                    {
                        if (rowsAffected != 1)
                            throw new ApplicationException(string.Format("{0} event source rows inserted. 1 expected.", rowsAffected));
                    });
        }


        private IExecutableSyntax UpdateVersion(ITransactionSyntax tx, Guid eventSourceId, int prevVersion, int newVersion)
        {
            return tx.NonQuery(
                Queries.UpdateVersion,
                new Dictionary<string, object>()
                    {
                        {"id", eventSourceId},
                        {"prevVersion", prevVersion},
                        {"newVersion", newVersion}
                    },
                rowsAffected =>
                    {
                        if (rowsAffected != 1)
                            throw new ApplicationException(string.Format("{0} event source rows updated. 1 expected.", rowsAffected));
                    });
        }

        private IExecutableSyntax InsertEvents(IExecutableSyntax tx, Guid eventSourceId, IEnumerable<EventDescriptor> events)
        {
            foreach (var evnt in events)
                tx = InsertEvent(tx, eventSourceId, evnt);
            return tx;
        }

        private IExecutableSyntax InsertEvent(IExecutableSyntax tx, Guid eventSourceId, EventDescriptor evnt)
        {
            return tx.NonQuery(
                Queries.InsertEvent,
                new Dictionary<string, object>()
                    {
                        {"eventSourceId", evnt.Id},
                        {"version", evnt.Version},
                        {"typeName", evnt.EventType},
                        {"data", evnt.EventData}
                    },
                rowsAffected =>
                    {
                        if (rowsAffected != 1)
                            throw new ApplicationException(string.Format("{0} event rows inserted. 1 expected.", rowsAffected));
                    });
        }

    }
}
