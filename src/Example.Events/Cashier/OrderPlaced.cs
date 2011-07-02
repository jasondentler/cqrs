using System;
using Cqrs.Eventing;

namespace Example.Cashier
{
    public class OrderPlaced : Event
    {
        public Guid OrderId { get; private set; }
        public DiningLocation DiningLocation { get; private set; }
        public OrderItem[] OrderItems { get; private set; }
        public decimal Price { get; private set; }

        public OrderPlaced(
            Guid orderId, 
            DiningLocation diningLocation, 
            OrderItem[] orderItems,
            decimal price)
        {
            OrderId = orderId;
            DiningLocation = diningLocation;
            OrderItems = orderItems;
            Price = price;
        }
    }
}
