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


        [When(@"I add a large latte, skim milk, double shot")]
        public void WhenIAddALargeLatteSkimMilkDoubleShot()
        {
            var latteId = EventSourceHelper.GetId<Item>("Latte");
            var orderId = EventSourceHelper.GetId<Order>();
            var item = new OrderItem(
                latteId,
                new Dictionary<string, string>()
                    {
                        {"Size", "large"},
                        {"Milk", "skim"},
                        {"Shot", "double"}
                    }, 1);
            var cmd = new AddOrderItem(orderId, item);
            WhenHelper.When(cmd);
        }

        [When(@"I cancel the order")]
        public void WhenICancelTheOrder()
        {
            var orderId = EventSourceHelper.GetId<Order>();
            var cmd = new CancelOrder(orderId);
            WhenHelper.When(cmd);
        }

        [When(@"I pay for the order")]
        public void WhenIPayForTheOrder()
        {
            var orderId = EventSourceHelper.GetId<Order>();
            var cmd = new PayOrder(
                orderId,
                "Jason Dentler",
                "5444444444444444");
            WhenHelper.When(cmd);
        }

    }
}
