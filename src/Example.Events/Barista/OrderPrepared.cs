using System;
using Cqrs.Eventing;

namespace Example.Barista
{
    public class OrderPrepared : Event
    {

        public Guid OrderId { get; private set; }
        public DiningLocation DiningLocation { get; private set; }
        public OrderItem[] OrderItems { get; private set; }

        public OrderPrepared(
            Guid orderId,
            DiningLocation diningLocation,
            OrderItem[] orderItems)
        {
            OrderId = orderId;
            DiningLocation = diningLocation;
            OrderItems = orderItems;
        }

    }
}
