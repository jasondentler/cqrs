using Cqrs.Specs;
using SharpTestsEx;
using TechTalk.SpecFlow;

namespace Example.Menu
{
    [Binding]
    public class Then
    {

        [Then(@"the menu item is added")]
        public void ThenTheMenuItemIsAdded()
        {
            var e = ThenHelper.Event<ItemAdded>();
            e.Name.Should().Be.EqualTo("Coffee");
        }

        [Then(@"nothing else happens")]
        public void ThenNothingElseHappens()
        {
            ThenHelper.UncheckedEvents().Should().Be.Empty();
            ThenHelper.HasException().Should().Be.False();
        }    

    }
}
