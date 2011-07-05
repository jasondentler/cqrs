using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cqrs.EventStore;
using NUnit.Framework;

namespace Cqrs.Domain
{

    [TestFixture]
    public class RepositoryImplTests : RepositoryTests<Repository>
    {
        protected override IRepository GetInstance(IEventStore eventStore)
        {
            return new Repository(eventStore);
        }
    }
}
