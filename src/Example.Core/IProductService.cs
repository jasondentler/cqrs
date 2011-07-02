using System;
using System.Collections.Generic;

namespace Example
{
    public interface IProductService
    {

        IEnumerable<IProductInfo> GetProductInfo(IEnumerable<Guid> menuItemIds);
        IProductInfo GetProductInfo(Guid menuItemId);

    }

    public interface IProductInfo
    {

        Guid MenuItemId { get; }
        string Name { get; }
        decimal Price { get; }

    }
}
