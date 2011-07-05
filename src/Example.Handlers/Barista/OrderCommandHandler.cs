using System.Linq;
using Cqrs;
using Cqrs.Domain;

namespace Example.Barista
{
    public class OrderCommandHandler : 
        IHandle<QueueOrder>,
        IHandle<BeginPreparingOrder>,
        IHandle<FinishPreparingOrder>,
        IHandle<DeliverOrder>
    {
        private readonly IRepository _repository;

        public OrderCommandHandler(
            IRepository repository)
        {
            _repository = repository;
        }

        public void Handle(QueueOrder message)
        {
            var order = new Order(
                message.BaristaOrderId,
                message.CashierOrderId,
                message.DiningLocation,
                message.OrderItems);
            _repository.Save(order);
        }

        public void Handle(BeginPreparingOrder message)
        {
            var order = _repository.GetById<Order>(message.OrderId);
            order.BeginPreparing();
            _repository.Save(order);
        }

        public void Handle(FinishPreparingOrder message)
        {
            var order = _repository.GetById<Order>(message.OrderId);
            order.FinishPreparing();
            _repository.Save(order); 
        }

        public void Handle(DeliverOrder message)
        {
            var order = _repository.GetById<Order>(message.OrderId);
            order.Deliver();
            _repository.Save(order);
        }
    }
}
