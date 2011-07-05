using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cqrs.Domain;
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

        [Then(@"nothing happens")]
        public void ThenNothingHappens()
        {
            WhenHelper.Events.Should().Be.Empty();
            ThenHelper.HasException().Should().Be.False();
        }

        [Then(@"the aggregate state is invalid")]
        public void ThenTheAggregateStateIsInvalid()
        {
            var ex = ThenHelper.Exception<InvalidStateException>();
        }

        [Then(@"the error is ""(.*)""")]
        public void ThenTheErrorIs(string expectedMessage)
        {
            var actual = WhenHelper.Exception.Message;
            actual.Should().Be.EqualTo(expectedMessage);
        }
    
    }
}
