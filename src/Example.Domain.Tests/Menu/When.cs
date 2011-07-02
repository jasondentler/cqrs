using System;
using Cqrs.Specs;
using TechTalk.SpecFlow;

namespace Example.Menu
{
    [Binding]
    public class When
    {

        [When(@"I add coffee to the menu")]
        public void WhenIAddCoffeeToTheMenu()
        {
            WhenHelper.When(new AddItem(Guid.NewGuid(), "Coffee"));
        }

        [When(@"I add drink size customizations to coffee")]
        public void WhenIAddDrinkSizeCustomizationsToCoffee()
        {
            var itemId = EventSourceHelper.GetId<Item>();
            var cmd = new AddCustomization(
                itemId, "Size", new[] {"Short", "Tall", "Grande", "Venti"});
            WhenHelper.When(cmd);
        }


    }
}
