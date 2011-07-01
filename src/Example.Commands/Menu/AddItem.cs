using System;
using Cqrs.Commanding;

namespace Example.Menu
{
    public class AddItem : Command 
    {

        public Guid MenuItemId { get; private set; }
        public string Name { get; private set; }

        public AddItem(Guid menuItemId,
            string name)
        {
            MenuItemId = menuItemId;
            Name = name;
        }
    }
}
