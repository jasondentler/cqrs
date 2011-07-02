using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cqrs.Commanding;

namespace Example.Cashier
{
    public class PlaceOrder : Command
    {
        public Guid OrderId { get; private set; }
        public DiningLocation TakeAway { get; private set; }
        public OrderItem[] OrderItems { get; private set; }

        public PlaceOrder(
            Guid orderId, 
            DiningLocation takeAway, 
            params OrderItem[] orderItems)
        {
            OrderId = orderId;
            TakeAway = takeAway;
            OrderItems = orderItems;
        }
    }
}
