using System;
using Cqrs.Commanding;

namespace Example.Menu
{
    public class AddItem : Command 
    {

        public Guid MenuItemId { get; private set; }
        public string Name { get; private set; }
        public decimal Price { get; private set; }

        public AddItem(Guid menuItemId,
            string name, 
            decimal price)
        {
            MenuItemId = menuItemId;
            Name = name;
            Price = price;
        }
    }
}
