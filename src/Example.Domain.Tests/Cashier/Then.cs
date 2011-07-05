using System;
using System.Collections.Generic;
using System.Linq;
using Cqrs.Specs;
using Example.Barista;
using Example.Menu;
using SharpTestsEx;
using TechTalk.SpecFlow;

namespace Example.Cashier
{
    [Binding]
    public class Then
    {

        [Then(@"a take-away order is placed for one small latte, whole milk")]
        public void ThenATake_AwayOrderIsPlacedForOneSmallLatteWholeMilk()
        {
            var latteId = EventSourceHelper.GetId<Item>("Latte");

            var orderItem = new OrderItem(
                latteId,
                new Dictionary<string, string>()
                    {
                        {"Size", "small"},
                        {"Milk", "milk"}
                    },
                1);

            var e = ThenHelper.Event<OrderPlaced>();
            e.DiningLocation.Should().Be.EqualTo(DiningLocation.TakeAway);
            e.OrderItems.Length.Should().Be.EqualTo(1);
            e.OrderItems[0].Should().Be.EqualTo(orderItem);
        }

        [Then(@"the price is \$7\.60")]
        public void ThenThePriceIs7_60()
        {
            var e = ThenHelper.Event<OrderPlaced>();
            e.Price.Should().Be.EqualTo(7.6M);
        }

        [Then(@"the updated order has two items")]
        public void ThenTheUpdatedOrderHasTwoItems()
        {
            var e = ThenHelper.Event<OrderItemAdded>();
            var allItems = e.ExistingItems.Union(new[] {e.AddedItem});
            allItems.Count().Should().Be.EqualTo(2);
        }

        [Then(@"the updated order includes a small latte, whole milk")]
        public void ThenTheUpdatedOrderIncludesASmallLatteWholeMilk()
        {
            var latteId = EventSourceHelper.GetId<Item>("Latte");
            var item = new OrderItem(
                latteId,
                new Dictionary<string, string>()
                    {
                        {"Size", "small"},
                        {"Milk", "whote"}
                    }, 1);
            var e = ThenHelper.Event<OrderItemAdded>();
            var allItems = e.ExistingItems.Union(new[] { e.AddedItem });
            allItems.Should().Contain(item);
        }

        [Then(@"the updated order includes a large latte, skim milk, double shot")]
        public void ThenTheUpdatedOrderIncludesALargeLatteSkimMilkDoubleShot()
        {
            var latteId = EventSourceHelper.GetId<Item>("Latte");
            var item = new OrderItem(
                latteId,
                new Dictionary<string, string>()
                    {
                        {"Size", "large"},
                        {"Milk", "whote"},
                        {"Shot", "double"}
                    }, 1);
            var e = ThenHelper.Event<OrderItemAdded>();
            var allItems = e.ExistingItems.Union(new[] { e.AddedItem });
            allItems.Should().Contain(item);
        }

        [Then(@"the updated order total is \$15\.20")]
        public void ThenTheUpdatedOrderTotalIs15_20()
        {
            var e = ThenHelper.Event<OrderItemAdded>();
            e.Price.Should().Be.EqualTo(15.20M);
        }

        [Then(@"the order is cancelled")]
        public void ThenTheOrderIsCancelled()
        {
            var orderId = EventSourceHelper.GetId<Order>();
            var e = ThenHelper.Event<OrderCancelled>();
            e.OrderId.Should().Be.EqualTo(orderId);
        }

        [Then(@"the order is paid for")]
        public void ThenTheOrderIsPaidFor()
        {
            var orderId = EventSourceHelper.GetId<Order>();
            var e = ThenHelper.Event<OrderPaid, Order>();
            var placedEvent = GivenHelper.GivenEvents(orderId).OfType<OrderPlaced>().Single();

            e.OrderId.Should().Be.EqualTo(orderId);
            e.CardHolderName.Should().Be.EqualTo("Jason Dentler");
            e.CardNumber.Should().Be.EqualTo("5444444444444444");
            e.DiningLocation.Should().Be.EqualTo(placedEvent.DiningLocation);
            e.OrderItems.Should().Have.SameValuesAs(placedEvent.OrderItems);
            e.Price.Should().Be.EqualTo(placedEvent.Price);
        }

        [Then(@"the cashier queues the order to the barista")]
        public void ThenTheCashierQueuesTheOrderToTheBarista()
        {
            var orderId = EventSourceHelper.GetId<Order>();
            var cmd = ThenHelper.Command<QueueOrder>();
            var placedEvent = GivenHelper.GivenEvents(orderId).OfType<OrderPlaced>().Single();

            cmd.CashierOrderId.Should().Be.EqualTo(orderId);
            cmd.DiningLocation.Should().Be.EqualTo(placedEvent.DiningLocation);
            cmd.OrderItems.Should().Have.SameValuesAs(placedEvent.OrderItems);

            cmd.BaristaOrderId.Should().Not.Be.EqualTo(Guid.Empty);
            cmd.BaristaOrderId.Should().Not.Be.EqualTo(orderId);
        }

    }
}
