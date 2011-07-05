using System;
using Cqrs.Commanding;

namespace Example.Cashier
{
    public class PayOrder : Command 
    {
        public Guid OrderId { get; private set; }
        public string CardHolderName { get; private set; }
        public string CardNumber { get; private set; }

        public PayOrder(
            Guid orderId,
            string cardHolderName,
            string cardNumber)
        {
            OrderId = orderId;
            CardHolderName = cardHolderName;
            CardNumber = cardNumber;
        }
    }
}
