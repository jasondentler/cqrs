using System;
using System.Linq;
using Cqrs.Domain;
using Stateless;

namespace Example.Cashier
{
    public class Order : AggregateRoot 
    {

        private enum OrderState
        {
            Initial,
            Placed, 
            Paid
        }

        private enum OrderAction
        {
            PlaceOrder,
            Pay
        }
        
        private readonly StateMachine<OrderState, OrderAction> _stateMachine;

        public Order()
        {
            _stateMachine = new StateMachine<OrderState, OrderAction>(
                OrderState.Initial);
            _stateMachine.Configure(OrderState.Initial)
                .Permit(OrderAction.PlaceOrder, OrderState.Placed);
            _stateMachine.Configure(OrderState.Placed)
                .Permit(OrderAction.Pay, OrderState.Paid);
        }

        public Order(
            Guid orderId, 
            DiningLocation diningLocation, 
            OrderItem[] orderItems, 
            IProductInfo[] products)
            : this()
        {
            if (!_stateMachine.CanFire(OrderAction.PlaceOrder)) return;

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
            _stateMachine.Fire(OrderAction.PlaceOrder);
        }

    }
}
