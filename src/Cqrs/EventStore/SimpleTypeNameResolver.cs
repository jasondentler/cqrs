using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cqrs.EventStore
{
    public class SimpleTypeNameResolver : ITypeNameResolver
    {
        public string GetNameFor(Type type)
        {
            return type.AssemblyQualifiedName;
        }

        public Type Resolve(string typeName)
        {
            return Type.GetType(typeName);
        }
    }
}
