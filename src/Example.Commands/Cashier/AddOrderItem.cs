using System;
using Cqrs.Commanding;

namespace Example.Cashier
{

    public class AddOrderItem : Command
    {
        public Guid OrderId { get; private set; }
        public OrderItem Item { get; private set; }

        public AddOrderItem(
            Guid orderId,
            OrderItem item)
        {
            OrderId = orderId;
            Item = item;
        }
    }

}
