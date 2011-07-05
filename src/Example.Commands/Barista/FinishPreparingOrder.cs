using System;
using Cqrs.Commanding;

namespace Example.Barista
{
    public class FinishPreparingOrder : Command
    {

        public Guid OrderId { get; private set; }

        public FinishPreparingOrder(
            Guid orderId)
        {
            OrderId = orderId;
        }
    }
}
