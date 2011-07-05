using Cqrs.Eventing;
using Newtonsoft.Json;

namespace Cqrs.EventStore.MsSql
{
    public class JsonSerializer : ISerializer<EventDescriptor>
    {
        private readonly ITypeNameResolver _resolver;

        public JsonSerializer(ITypeNameResolver resolver)
        {
            _resolver = resolver;
        }

        public EventDescriptor Serialize(EventStore.EventDescriptor descriptor)
        {
            var e = descriptor.EventData;
            var data = JsonConvert.SerializeObject(e);
            var type = _resolver.GetNameFor(e.GetType());
            return new EventDescriptor(data, type, descriptor.Id, descriptor.Version);
        }

        public EventStore.EventDescriptor Deserialize(EventDescriptor serializedMessage)
        {
            var data = serializedMessage.EventData;
            var typeName = serializedMessage.EventType;
            var type = _resolver.Resolve(typeName);
            var e = (Event) JsonConvert.DeserializeObject(data, type);
            return new EventStore.EventDescriptor(serializedMessage.Id, e, serializedMessage.Version);
        }
    }
}
