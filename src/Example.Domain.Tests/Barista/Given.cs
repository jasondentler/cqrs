using System;
using System.Collections.Generic;
using System.Linq;
using Cqrs.Specs;
using Example.Menu;
using TechTalk.SpecFlow;

namespace Example.Barista
{
    [Binding]
    public class Given
    {

        [Given(@"the cashier has queued an order for a small latte, whole milk")]
        public void GivenTheCashierHasQueuedAnOrderForASmallLatteWholeMilk()
        {
            var latteId = EventSourceHelper.GetId<Item>("Latte");

            var e = new OrderQueued(
                Guid.NewGuid(),
                Guid.NewGuid(),
                DiningLocation.TakeAway,
                new OrderItem(
                    latteId,
                    new Dictionary<string, string>()
                        {
                            {"Size", "small"},
                            {"Milk", "whole"}
                        },
                    1)
                );

            GivenHelper.Given(e.BaristaOrderId, e);
        }

        [Given(@"I have begun preparing the order")]
        public void GivenIHaveBegunPreparingTheOrder()
        {
            var orderId = EventSourceHelper.GetId<Order>();
            var orderQueued = GivenHelper.GivenEvents(orderId)
                .OfType<OrderQueued>().Single();

            var e = new OrderBeingPrepared(
                orderId,
                orderQueued.DiningLocation,
                orderQueued.OrderItems);
            GivenHelper.Given(orderId, e);
        }

        [Given(@"I have prepared the order")]
        public void GivenIHavePreparedTheOrder()
        {
            var orderId = EventSourceHelper.GetId<Order>();
            var orderQueued = GivenHelper.GivenEvents(orderId)
                .OfType<OrderQueued>().Single();

            var e = new OrderPrepared(
                orderId,
                orderQueued.DiningLocation,
                orderQueued.OrderItems);
            GivenHelper.Given(orderId, e);
        }

    }
}
