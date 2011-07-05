using System;
using Cqrs.Commanding;

namespace Example.Cashier
{
    public class CancelOrder : Command
    {

        public Guid OrderId { get; private set; }

        public CancelOrder(Guid orderId)
        {
            OrderId = orderId;
        }
    }
}
