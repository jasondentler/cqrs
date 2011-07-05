using System;
using Cqrs.Eventing;

namespace Example.Barista
{
    public class OrderQueued : Event
    {

        public Guid CashierOrderId { get; private set; }
        public Guid BaristaOrderId { get; private set; }
        public DiningLocation DiningLocation { get; private set; }
        public OrderItem[] OrderItems { get; private set; }

        public OrderQueued(
            Guid cashierOrderId,
            Guid baristaOrderId,
            DiningLocation diningLocation,
            params OrderItem[] orderItems)
        {
            CashierOrderId = cashierOrderId;
            BaristaOrderId = baristaOrderId;
            DiningLocation = diningLocation;
            OrderItems = orderItems;
        }

    }
}
