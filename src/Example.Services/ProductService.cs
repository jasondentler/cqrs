using System;
using System.Collections.Generic;
using System.Linq;
using Cqrs;
using Example.Menu;

namespace Example.Services
{
    public class ProductService : 
        IProductService,
        IHandle<ItemAdded>
    {

        private readonly Dictionary<Guid, IProductInfo> _items = new Dictionary<Guid, IProductInfo>();

        public IEnumerable<IProductInfo> GetProductInfo(IEnumerable<Guid> menuItemIds)
        {
            return menuItemIds
                .Distinct()
                .Select(GetProductInfo);
        }

        public IProductInfo GetProductInfo(Guid menuItemId)
        {
            return _items[menuItemId];
        }

        public void Handle(ItemAdded message)
        {
            _items[message.MenuItemId] = new ProductInfo(
                message.MenuItemId,
                message.Name,
                message.Price);
        }
    }

    public class ProductInfo : IProductInfo
    {
        public Guid MenuItemId { get; private set; }
        public string Name { get; private set; }
        public decimal Price { get; private set; }

        public ProductInfo(
            Guid menuItemId,
            string name,
            decimal price)
        {
            MenuItemId = menuItemId;
            Name = name;
            Price = price;
        }
    }

}
