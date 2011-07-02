using System;
using Cqrs.Eventing;

namespace Example.Menu
{
    public class ItemAdded : Event
    {

        public Guid MenuItemId { get; private set; }
        public string Name { get; private set; }

        public ItemAdded(
            Guid menuItemId,
            string name)
        {
            MenuItemId = menuItemId;
            Name = name;
        }
    }
}
