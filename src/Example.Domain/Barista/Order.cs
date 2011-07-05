using System;
using System.Collections.Generic;
using Cqrs.Domain;

namespace Example.Barista
{
    public class Order : AggregateRoot
    {

        private enum State
        {
            Initial,
            Queued,
            BeingPrepared,
            Prepared,
            Delivered
        }

        private State _state = State.Initial;
        private DiningLocation _location;
        private List<OrderItem> _items = new List<OrderItem>();

        public Order()
        {
        }

        public Order(
            Guid orderId,
            Guid cashierOrderId,
            DiningLocation location,
            OrderItem[] items)
        {
            var e = new OrderQueued(
                cashierOrderId,
                orderId,
                location,
                items);
            ApplyChange(e);
        }

        public void BeginPreparing()
        {
            var e = new OrderBeingPrepared(
                Id,
                _location,
                _items.ToArray());
            ApplyChange(e);
        }

        public void FinishPreparing()
        {
            if (_state == State.Queued)
                BeginPreparing();

            var e = new OrderPrepared(
                Id,
                _location,
                _items.ToArray());
            ApplyChange(e);
        }

        public void Deliver()
        {

            if (_state == State.Queued || _state == State.BeingPrepared)
                FinishPreparing();

            var e = new OrderDelivered(
                Id,
                _location,
                _items.ToArray());
            ApplyChange(e);
        }

        private void Apply(OrderQueued e)
        {
            _state = State.Queued;
            Id = e.BaristaOrderId;
            _location = e.DiningLocation;
            _items.AddRange(e.OrderItems);
        }

        private void Apply(OrderBeingPrepared e)
        {
            _state = State.BeingPrepared;
        }

        private void Apply(OrderPrepared e)
        {
            _state = State.Prepared;
        }

        private void Apply(OrderDelivered e)
        {
            _state = State.Delivered;
        }

    }
}
