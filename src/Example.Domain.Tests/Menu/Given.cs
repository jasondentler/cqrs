using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            var e = new ItemAdded(Guid.NewGuid(), "Coffee");
            GivenHelper.Given(e.MenuItemId, e);
        }
    
    }
}
