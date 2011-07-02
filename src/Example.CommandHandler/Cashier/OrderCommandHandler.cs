using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cqrs;
using Cqrs.Domain;

namespace Example.Cashier
{
    public class OrderCommandHandler : 
        IHandle<PlaceOrder>
    {
        private readonly IRepository<Order> _repository;

        public OrderCommandHandler(IRepository<Order> repository)
        {
            _repository = repository;
        }

        public void Handle(PlaceOrder message)
        {

            var order = new Order(
                message.OrderId,
                message.TakeAway,
                message.OrderItems);
            _repository.Save(order, 0);


        }
    }
}
