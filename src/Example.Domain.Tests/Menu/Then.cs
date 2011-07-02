using Cqrs.Specs;
using SharpTestsEx;
using TechTalk.SpecFlow;

namespace Example.Menu
{
    [Binding]
    public class Then
    {

        [Then(@"coffee is added to the menu")]
        public void ThenCoffeeIsAddedToTheMenu()
        {
            var e = ThenHelper.Event<ItemAdded>();
            e.Name.Should().Be.EqualTo("Coffee");
        }

        [Then(@"drink size customizations are added to coffee")]
        public void ThenDrinkSizeCustomizationsAreAddedToCoffee()
        {
            var menuItemId = EventSourceHelper.GetId<Item>("Coffee");
            var options = new[] {"Short", "Tall", "Grande", "Venti"};
            var e = ThenHelper.Event<CustomizationAdded>();

            e.MenuItemId.Should().Be.EqualTo(menuItemId);
            e.Name.Should().Be.EqualTo("Size");
            e.Options.Should().Have.SameValuesAs(options);

        }





    }
}
