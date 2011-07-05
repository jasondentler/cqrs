using System;
using Cqrs.Eventing;

namespace Example.Cashier
{
    public class OrderItemAdded : Event
    {
        public Guid OrderId { get; private set; }
        public DiningLocation DiningLocation { get; private set; }
        public OrderItem[] ExistingItems { get; private set; }
        public OrderItem AddedItem { get; private set; }
        public decimal Price { get; private set; }

        public OrderItemAdded(
            Guid orderId, 
            DiningLocation diningLocation, 
            OrderItem[] existingItems,
            OrderItem addedItem,
            decimal price)
        {
            OrderId = orderId;
            DiningLocation = diningLocation;
            ExistingItems = existingItems;
            AddedItem = addedItem;
            Price = price;
        }
    }
}
