using System;
using System.Collections.Generic;
using System.Linq;
using Cqrs.Domain;

namespace Example.Cashier
{
    public class Order : AggregateRoot 
    {

        private enum OrderState
        {
            Initial,
            Placed, 
            Paid,
            Cancelled
        }

        private OrderState _state = OrderState.Initial;
        private readonly List<OrderItem> _items = new List<OrderItem>();
        private decimal _price = 0M;
        private DiningLocation _location;
        
        public Order()
        {
        }

        public Order(
            Guid orderId, 
            DiningLocation diningLocation, 
            OrderItem[] orderItems, 
            IProductInfo[] products)
            : this()
        {
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

        public void AddItem(OrderItem item, IProductInfo product)
        {
            switch (_state)
            {
                case OrderState.Cancelled:
                    throw new InvalidStateException(
                        "You can't add an item. This order is already cancelled. Place a new order.");
                case OrderState.Paid:
                    throw new InvalidStateException("You can't change this order. It's already paid. Place a new order.");
            }

            var priceForNewItem = item.Quantity*product.Price;
            var e = new OrderItemAdded(
                Id,
                _location,
                _items.ToArray(),
                item,
                _price + priceForNewItem);
            ApplyChange(e);
        }

        public void Cancel()
        {

            switch (_state)
            {
                case OrderState.Cancelled:
                    return;
                case OrderState.Paid:
                    throw new InvalidStateException("You can't cancel this order. It has already been paid.");
            }

            var e = new OrderCancelled(Id);
            ApplyChange(e);
        }


        public void Pay(string cardHolderName, string cardNumber)
        {

            switch (_state)
            {
                case OrderState.Cancelled:
                    throw new InvalidStateException("You can't pay for this order. It is cancelled. Place a new order.");
                case OrderState.Paid:
                    throw new InvalidStateException("You can't pay for this order. It's already paid.");
            }

            var e = new OrderPaid(
                Id,
                _location,
                _items.ToArray(),
                _price,
                cardHolderName,
                cardNumber,
                Guid.NewGuid(),
                Guid.NewGuid());
            ApplyChange(e);
        }

        private void Apply(OrderPlaced e)
        {
            Id = e.OrderId;
            _state = OrderState.Placed;
            _items.AddRange(e.OrderItems);
            _price = e.Price;
            _location = e.DiningLocation;
        }

        private void Apply(OrderItemAdded e)
        {
            _price = e.Price;
            _items.Add(e.AddedItem);
        }

        private void Apply(OrderCancelled e)
        {
            _state = OrderState.Cancelled;
        }

        private void Apply(OrderPaid e)
        {
            _state = OrderState.Paid;
        }
    }
}
