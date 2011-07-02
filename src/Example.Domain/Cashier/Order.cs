using System;
using System.Collections.Generic;
using System.Linq;
using Cqrs.Domain;
using Stateless;

namespace Example.Cashier
{
    public class Order : AggregateRoot 
    {

        public enum OrderState
        {
            Initial,
            Placed, 
            Paid
        }

        public enum OrderTriggers
        {
            PlaceOrder,
            Pay
        }

        private readonly StateMachine<OrderState, OrderTriggers> _stateMachine;

        public Order()
        {
            _stateMachine = new StateMachine<OrderState, OrderTriggers>(
                OrderState.Initial);
            _stateMachine.Configure(OrderState.Initial)
                .Permit(OrderTriggers.PlaceOrder, OrderState.Placed);
            _stateMachine.Configure(OrderState.Placed)
                .Permit(OrderTriggers.Pay, OrderState.Paid);
        }

        public Order(
            Guid orderId, 
            DiningLocation diningLocation, 
            OrderItem[] orderItems, 
            IProductInfo[] products)
            : this()
        {
            if (!_stateMachine.CanFire(OrderTriggers.PlaceOrder)) return;

            var price = orderItems
                .Select(i => new
                                 {
                                     product = products.Single(p => p.MenuItemId == i.MenuItemId),
                                     item = i
                                 })
                .Select(i => i.item.Quantity*i.product.Price)
                .Sum();

            var e = new OrderPlaced(
                orderId,
                diningLocation,
                orderItems,
                price);
            ApplyChange(e);
        }

        private void Apply(OrderPlaced e)
        {
            Id = e.OrderId;
            _stateMachine.Fire(OrderTriggers.PlaceOrder);
        }
    }
}
