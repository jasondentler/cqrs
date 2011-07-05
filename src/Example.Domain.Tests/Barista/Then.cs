using System;
using System.Linq;
using Cqrs.Specs;
using Example.Barista;
using Example.Cashier;
using SharpTestsEx;
using TechTalk.SpecFlow;

namespace Example.Barista
{
    [Binding]
    public class Then
    {

        [Then(@"the order is queued to the barista")]
        public void ThenTheOrderIsQueuedToTheBarista()
        {
            var baristaOrderId = EventSourceHelper.GetId<Barista.Order>();
            var cashierOrderId = EventSourceHelper.GetId<Cashier.Order>();
            var e = ThenHelper.Event<OrderQueued>();

            var placedEvent = GivenHelper.GivenEvents(cashierOrderId).OfType<OrderPlaced>().Single();

            e.BaristaOrderId.Should().Be.EqualTo(baristaOrderId);
            e.CashierOrderId.Should().Be.EqualTo(cashierOrderId);

            baristaOrderId.Should().Not.Be.EqualTo(cashierOrderId);

            e.DiningLocation.Should().Be.EqualTo(placedEvent.DiningLocation);
            e.OrderItems.Should().Have.SameValuesAs(placedEvent.OrderItems);

        }

        [Then(@"the order is being prepared")]
        public void ThenTheOrderIsBeingPrepared()
        {
            var baristaOrderId = EventSourceHelper.GetId<Order>();
            var queuedOrder = GivenHelper.GivenEvents(baristaOrderId)
                .OfType<OrderQueued>().Single();

            var e = ThenHelper.Event<OrderBeingPrepared>();
            e.DiningLocation.Should().Be.EqualTo(queuedOrder.DiningLocation);
            e.OrderId.Should().Be.EqualTo(baristaOrderId);
            e.OrderItems.Should().Have.SameValuesAs(queuedOrder.OrderItems);

        }

        [Then(@"the order is prepared")]
        public void ThenTheOrderIsPrepared()
        {
            var baristaOrderId = EventSourceHelper.GetId<Order>();
            var queuedOrder = GivenHelper.GivenEvents(baristaOrderId)
                .OfType<OrderQueued>().Single();

            var e = ThenHelper.Event<OrderPrepared>();
            e.DiningLocation.Should().Be.EqualTo(queuedOrder.DiningLocation);
            e.OrderId.Should().Be.EqualTo(baristaOrderId);
            e.OrderItems.Should().Have.SameValuesAs(queuedOrder.OrderItems);
        }

        [Then(@"the order is delivered")]
        public void ThenTheOrderIsDelivered()
        {
            var baristaOrderId = EventSourceHelper.GetId<Order>();
            var queuedOrder = GivenHelper.GivenEvents(baristaOrderId)
                .OfType<OrderQueued>().Single();

            var e = ThenHelper.Event<OrderDelivered>();
            e.DiningLocation.Should().Be.EqualTo(queuedOrder.DiningLocation);
            e.OrderId.Should().Be.EqualTo(baristaOrderId);
            e.OrderItems.Should().Have.SameValuesAs(queuedOrder.OrderItems);
        }

    }
}
