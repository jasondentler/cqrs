using System;
using Cqrs.Domain;

namespace Example.Menu
{
    public class Item : AggregateRoot 
    {

        public Item()
        {
        }

        public Item(
            Guid menuItemId,
            string name)
        {
            ApplyChange(new ItemAdded(menuItemId, name));
        }

        private void Apply(ItemAdded e)
        {
            Id = e.MenuItemId;
        }

    }
}
