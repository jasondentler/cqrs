using System.Collections.Generic;
using Cqrs.Specs;
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

    }
}
