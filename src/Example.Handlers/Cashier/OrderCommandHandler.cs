using System.Linq;
using Cqrs;
using Cqrs.Domain;

namespace Example.Cashier
{
    public class OrderCommandHandler : 
        IHandle<PlaceOrder>,
        IHandle<AddOrderItem>,
        IHandle<CancelOrder>,
        IHandle<PayOrder>
    {
        private readonly IRepository _repository;
        private readonly IProductService _productService;

        public OrderCommandHandler(
            IRepository repository,
            IProductService productService)
        {
            _repository = repository;
            _productService = productService;
        }

        public void Handle(PlaceOrder message)
        {

            var menuItemIds = message.OrderItems
                .Select(m => m.MenuItemId);

            var products = _productService.GetProductInfo(menuItemIds).ToArray();

            var order = new Order(
                message.OrderId,
                message.DiningLocation,
                message.OrderItems,
                products);
            _repository.Save(order);


        }

        public void Handle(AddOrderItem message)
        {
            var menuItemId = message.Item.MenuItemId;
            var product = _productService.GetProductInfo(menuItemId);
            var order = _repository.GetById<Order>(message.OrderId);
            order.AddItem(message.Item, product);
            _repository.Save(order);
        }

        public void Handle(CancelOrder message)
        {
            var order = _repository.GetById<Order>(message.OrderId);
            order.Cancel();
            _repository.Save(order);
        }

        public void Handle(PayOrder message)
        {
            var order = _repository.GetById<Order>(message.OrderId);
            order.Pay(message.CardHolderName, message.CardNumber);
            _repository.Save(order);
        }
    }
}
