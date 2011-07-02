using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cqrs;
using Cqrs.Specs;
using Example.Menu;

namespace Example
{
    public class RegisterEventSources 
        : IHandle<ItemAdded>
    {
        public void Handle(ItemAdded message)
        {
            Console.WriteLine("Registering new menu item {0}", message.Name);
            EventSourceHelper.SetId<Item>(message.MenuItemId, message.Name);
        }
    }
}
