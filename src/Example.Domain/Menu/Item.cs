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
            string name,
            decimal price)
        {
            ApplyChange(new ItemAdded(menuItemId, name, price));
        }

        public void AddCustomization(string name, string[] options)
        {
            ApplyChange(new CustomizationAdded(Id, name, options));
        }

        private void Apply(ItemAdded e)
        {
            Id = e.MenuItemId;
        }

        private void Apply(CustomizationAdded e)
        {
        }

    }
}
