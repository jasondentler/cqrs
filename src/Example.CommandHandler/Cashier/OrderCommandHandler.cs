using System.Linq;
using Cqrs;
using Cqrs.Domain;

namespace Example.Cashier
{
    public class OrderCommandHandler : 
        IHandle<PlaceOrder>
    {
        private readonly IRepository<Order> _repository;
        private readonly IProductService _productService;

        public OrderCommandHandler(
            IRepository<Order> repository,
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
                message.TakeAway,
                message.OrderItems,
                products);
            _repository.Save(order, 0);


        }
    }
}
