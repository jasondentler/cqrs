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

        [Then(@"the cost is \$3\.00")]
        public void ThenTheCostIs3_00()
        {
            var e = ThenHelper.Event<OrderPlaced>();
            e.Price.Should().Be.EqualTo(3M);
        }

        [Then(@"the order can be paid")]
        public void ThenTheOrderCanBePaid()
        {
            ScenarioContext.Current.Pending();
        }

    }
}
