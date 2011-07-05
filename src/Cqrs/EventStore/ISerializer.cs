namespace Cqrs.EventStore
{

    public interface ISerializer<TStorageType>
    {

        TStorageType Serialize(EventDescriptor descriptor);
        EventDescriptor Deserialize(TStorageType serializedMessage);
        
    }

}
