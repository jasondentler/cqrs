using System;
using Cqrs.Eventing;

namespace Example.Menu
{
    public class CustomizationAdded : Event
    {
        public Guid MenuItemId { get; private set; }
        public string Name { get; private set; }
        public string[] Options { get; private set; }

        public CustomizationAdded(
            Guid menuItemId,
            string name,
            string[] options)
        {
            MenuItemId = menuItemId;
            Name = name;
            Options = options;
        }
    }
}
