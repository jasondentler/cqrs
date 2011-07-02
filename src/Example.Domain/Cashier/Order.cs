using System;
using Cqrs.Domain;

namespace Example.Cashier
{
    public class Order : AggregateRoot 
    {

        public Order()
        {
        }

        public Order(
            Guid orderId, 
            DiningLocation diningLocation, 
            OrderItem[] orderItems)
        {
            var e = new OrderPlaced(
                orderId,
                diningLocation,
                orderItems,
                0M);
            ApplyChange(e);
        }

        private void Apply(OrderPlaced e)
        {
            Id = e.OrderId;
        }
    }
}
