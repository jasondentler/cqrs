using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cqrs;
using Cqrs.Specs;
using Example.Barista;
using Example.Cashier;
using Example.Menu;
using Order = Example.Cashier.Order;

namespace Example
{
    public class RegisterEventSources  :
        IHandle<ItemAdded>,
        IHandle<OrderPlaced>,
        IHandle<OrderQueued>
    {
        public void Handle(ItemAdded message)
        {
            Console.WriteLine("Registering new menu item {0}", message.Name);
            EventSourceHelper.SetId<Item>(message.MenuItemId, message.Name);
        }

        public void Handle(OrderPlaced message)
        {
            Console.WriteLine("Registering a new placed order");
            EventSourceHelper.SetId<Order>(message.OrderId);
        }

        public void Handle(OrderQueued message)
        {
            Console.WriteLine("Registering a new barista order");
            EventSourceHelper.SetId<Barista.Order>(message.BaristaOrderId);
        }
    }
}
