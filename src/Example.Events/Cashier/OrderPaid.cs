using System;
using Cqrs.Eventing;

namespace Example.Cashier
{
    public class OrderPaid : Event
    {

        public Guid OrderId { get; private set; }
        public DiningLocation DiningLocation { get; private set; }
        public OrderItem[] OrderItems { get; private set; }
        public decimal Price { get; private set; }
        public string CardHolderName { get; private set; }
        public string CardNumber { get; private set; }
        public Guid BaristaOrderId { get; private set; }
        public Guid CoordinatorId { get; private set; }

        public OrderPaid(
            Guid orderId, 
            DiningLocation diningLocation, 
            OrderItem[] orderItems,
            decimal price,
            string cardHolderName,
            string cardNumber,
            Guid baristaOrderId,
            Guid coordinatorId)
        {
            OrderId = orderId;
            DiningLocation = diningLocation;
            OrderItems = orderItems;
            Price = price;
            CardHolderName = cardHolderName;
            CardNumber = cardNumber;
            BaristaOrderId = baristaOrderId;
            CoordinatorId = coordinatorId;
        }

    }
}
