using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cqrs.Eventing;

namespace Example
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
