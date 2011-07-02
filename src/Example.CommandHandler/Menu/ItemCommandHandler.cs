using Cqrs;
using Cqrs.Domain;

namespace Example.Menu
{
    public class ItemCommandHandler : 
        IHandle<AddItem>,
        IHandle<AddCustomization>
    {
        private readonly IRepository<Item> _repository;

        public ItemCommandHandler(IRepository<Item> repository)
        {
            _repository = repository;
        }

        public void Handle(AddItem message)
        {
            var item = new Item(message.MenuItemId, message.Name);
            _repository.Save(item, 0);
        }

        public void Handle(AddCustomization message)
        {
            var item = _repository.GetById(message.MenuItemId);
            item.AddCustomization(message.Name, message.Options);
            _repository.Save(item, item.Version);
        }
    }
}
