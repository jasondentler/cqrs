using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cqrs.Specs;
using SharpTestsEx;
using TechTalk.SpecFlow;

namespace Example
{
    [Binding]
    public class Then
    {

        [Then(@"nothing else happens")]
        public void ThenNothingElseHappens()
        {
            ThenHelper.UncheckedEvents().Should().Be.Empty();
            ThenHelper.HasException().Should().Be.False();
        }        
    
    }
}
