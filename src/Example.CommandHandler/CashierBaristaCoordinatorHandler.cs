using Cqrs;
using Cqrs.Domain;
using Example.Cashier;

namespace Example
{
    public class CashierBaristaCoordinatorHandler
        : IHandle<OrderPaid>
    {
        private readonly IRepository<CashierBaristaCoordinator> _repository;

        public CashierBaristaCoordinatorHandler(
            IRepository<CashierBaristaCoordinator> repository)
        {
            _repository = repository;
        }

        public void Handle(OrderPaid message)
        {
            var saga = new CashierBaristaCoordinator(message);
            _repository.Save(saga);
        }

    }
}
