using Cqrs.Specs;
using TechTalk.SpecFlow;

namespace Example.Barista
{
    [Binding]
    public class When
    {

        [When(@"I begin preparing the order")]
        public void WhenIBeginPreparingTheOrder()
        {
            var orderId = EventSourceHelper.GetId<Order>();
            var cmd = new BeginPreparingOrder(orderId);
            WhenHelper.When(cmd);
        }

        [When(@"I prepare the order")]
        public void WhenIPrepareTheOrder()
        {
            var orderId = EventSourceHelper.GetId<Order>();
            var cmd = new FinishPreparingOrder(orderId);
            WhenHelper.When(cmd);
        }

        [When(@"I deliver the order")]
        public void WhenIDeliverTheOrder()
        {
            var orderId = EventSourceHelper.GetId<Order>();
            var cmd = new DeliverOrder(orderId);
            WhenHelper.When(cmd);
        }

    }
}
