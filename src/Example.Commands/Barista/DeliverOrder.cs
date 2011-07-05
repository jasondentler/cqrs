using System;
using Cqrs.Commanding;

namespace Example.Barista
{
    public class DeliverOrder : Command
    {

        public Guid OrderId { get; private set; }

        public DeliverOrder(
            Guid orderId)
        {
            OrderId = orderId;
        }
    }
}
