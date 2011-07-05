using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cqrs.EventStore.MsSql
{
    internal static class Queries
    {

        public const string SelectVersion = "SELECT [Version] FROM [EventSources] WHERE [Id] = @id";
        public const string SelectAllEvents = "SELECT [Version], [TypeName], [Data] FROM [Events] WHERE [EventSourceId] = @id ORDER BY [Version]";
        public const string UpdateVersion = "UPDATE [EventSources] SET [Version] = @newVersion WHERE [Id] = @id AND [Version] = @prevVersion";
        public const String InsertEvent = "INSERT INTO [Events]([EventSourceId], [Version], [TypeName], [Data]) VALUES (@eventSourceId, @version, @typeName, @data)";
        public const String InsertEventSource = "INSERT INTO [EventSources](Id, Version) VALUES (@id, @newVersion)";


    }
}
