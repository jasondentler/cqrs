using System.Collections.Generic;
using Cqrs.Commanding;
using Cqrs.Domain;

namespace Cqrs.Sagas
{
    public abstract class Saga : EventSource, ISaga
    {

        private readonly List<Command> _dispatches = new List<Command>();

        protected void Dispatch(Command dispatch)
        {
            _dispatches.Add(dispatch);
        }

        public virtual IEnumerable<Command> GetDispatches()
        {
            return _dispatches;
        }

        public override void MarkChangesAsCommitted()
        {
            base.MarkChangesAsCommitted();
            _dispatches.Clear();
        }


    }
}
