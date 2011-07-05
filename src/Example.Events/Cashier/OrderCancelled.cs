using System;
using Cqrs.Eventing;

namespace Example.Cashier
{
    public class OrderCancelled : Event
    {
        public Guid OrderId { get; private set; }

        public OrderCancelled(
            Guid orderId)
        {
            OrderId = orderId;
        }
    }
}
