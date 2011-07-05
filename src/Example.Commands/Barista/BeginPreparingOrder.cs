using System;
using Cqrs.Commanding;

namespace Example.Barista
{
    public class BeginPreparingOrder : Command
    {

        public Guid OrderId { get; private set; }

        public BeginPreparingOrder(
            Guid orderId)
        {
            OrderId = orderId;
        }
    }
}
