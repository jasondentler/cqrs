using System;
using Cqrs.Specs;
using TechTalk.SpecFlow;

namespace Example.Menu
{
    [Binding]
    public class Given
    {

        [Given(@"I have add coffee to the menu")]
        public void GivenIHaveAddCoffeeToTheMenu()
        {
            var e = new ItemAdded(Guid.NewGuid(), "Coffee", 2M);
            GivenHelper.Given(e.MenuItemId, e);
        }
    
    }
}
