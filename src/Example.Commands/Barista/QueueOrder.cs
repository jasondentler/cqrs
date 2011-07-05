using System;
using Cqrs.Commanding;

namespace Example.Barista
{
    public class QueueOrder : Command
    {

        public Guid CashierOrderId { get; private set; }
        public Guid BaristaOrderId { get; private set; }
        public DiningLocation DiningLocation { get; private set; }
        public OrderItem[] OrderItems { get; private set; }

        public QueueOrder(
            Guid baristaOrderId,
            Guid cashierOrderId,
            DiningLocation diningLocation,
            OrderItem[] orderItems)
        {
            BaristaOrderId = baristaOrderId;
            CashierOrderId = cashierOrderId;
            DiningLocation = diningLocation;
            OrderItems = orderItems;
        }

    }
}
