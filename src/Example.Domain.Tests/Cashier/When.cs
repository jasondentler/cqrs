using System;
using System.Collections.Generic;
using Cqrs.Specs;
using Example.Menu;
using TechTalk.SpecFlow;

namespace Example.Cashier
{
    [Binding]
    public class When
    {

        [When(@"a customer places a take-away order for one small latte, whole milk")]
        public void WhenACustomerPlacesATake_AwayOrderForOneSmallLatteWholeMilk()
        {
            var latteId = EventSourceHelper.GetId<Item>("Latte");

            var cmd = new PlaceOrder(
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

            WhenHelper.When(cmd);
        }
    
    }
}
