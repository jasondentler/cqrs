using System;
using Cqrs.Specs;
using TechTalk.SpecFlow;

namespace Example.Menu
{
    [Binding]
    public class When
    {

        [When(@"I add a menu item")]
        public void WhenIAddAMenuItem()
        {
            WhenHelper.When(new AddItem(Guid.NewGuid(), "Coffee"));
        }

    }
}
