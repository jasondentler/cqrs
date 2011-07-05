using System;

namespace Cqrs.EventStore
{
    public interface ITypeNameResolver
    {

        string GetNameFor(Type type);
        Type Resolve(string typeName);

    }
}
