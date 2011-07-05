using Cqrs.Sagas;
using Example.Barista;
using Example.Cashier;

namespace Example
{

    public class CashierBaristaCoordinator : Saga
    {

        public CashierBaristaCoordinator()
        {
        }

        public CashierBaristaCoordinator(OrderPaid message)
        {
            var e = new OrderPaid(
                message.OrderId,
                message.DiningLocation,
                message.OrderItems,
                message.Price,
                message.CardHolderName,
                message.CardNumber,
                message.BaristaOrderId,
                message.CoordinatorId);

            ApplyChange(e);
            Dispatch(new QueueOrder(
                         e.BaristaOrderId,
                         e.OrderId,
                         e.DiningLocation,
                         e.OrderItems));
        }

        private void Apply(OrderPaid e)
        {
            Id = e.CoordinatorId;
        }

    }

}
