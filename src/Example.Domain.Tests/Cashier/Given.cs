using System;
using System.Collections.Generic;
using System.Linq;
using Cqrs.Specs;
using Example.Menu;
using TechTalk.SpecFlow;
using log4net.Util.TypeConverters;

namespace Example.Cashier
{
    [Binding]
    public class Given
    {

        [Given(@"I have placed an order for a small latte, whole milk")]
        public void GivenIHavePlacedAnOrderForASmallLatteWholeMilk()
        {
            var latteId = EventSourceHelper.GetId<Item>("Latte");
            var orderId = Guid.NewGuid();

            var e = new OrderPlaced(
                orderId,
                DiningLocation.TakeAway,
                new OrderItem[1]
                    {
                        new OrderItem(
                            latteId,
                            new Dictionary<string, string>()
                                {
                                    {"Size", "small"},
                                    {"Milk", "whole"}
                                }, 1)
                    },
                7.60M);

            GivenHelper.Given(orderId, e);
        }

        [Given(@"I have cancelled the order")]
        public void GivenIHaveCancelledTheOrder()
        {
            var orderId = EventSourceHelper.GetId<Order>();
            var e = new OrderCancelled(orderId);
            GivenHelper.Given(orderId, e);
        }

        [Given(@"I have paid for the order")]
        public void GivenIHavePaidForTheOrder()
        {
            var orderId = EventSourceHelper.GetId<Order>();
            var orderEvents = GivenHelper.GivenEvents(orderId).ToArray();
            var orderPlacedEvent = orderEvents.OfType<OrderPlaced>().Single();
            if (orderEvents.Last() != orderPlacedEvent)
                throw new NotSupportedException("Sorry. This test helper isn't set up to handle updated orders.");

            var e = new OrderPaid(
                orderId,
                orderPlacedEvent.DiningLocation,
                orderPlacedEvent.OrderItems,
                orderPlacedEvent.Price,
                "Jason Dentler",
                "5444444444444444",
                Guid.NewGuid(),
                Guid.NewGuid());
            GivenHelper.Given(orderId, e);
        }


    }
}
