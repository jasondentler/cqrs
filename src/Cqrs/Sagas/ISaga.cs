using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cqrs.Commanding;
using Cqrs.Domain;

namespace Cqrs.Sagas
{
    public interface ISaga : IEventSource
    {

        IEnumerable<Command> GetDispatches();

    }
}
