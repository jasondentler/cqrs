using System;
using System.Collections.Generic;
using Cqrs.EventStore;
using Cqrs.Eventing;
using NUnit.Framework;
using SharpTestsEx;

namespace Cqrs.Domain
{
    [TestFixture]
    public class EventSourceTests
    {

        [Test]
        public void NewEventsAreAppliedToTheAggregate()
        {
            E appliedEvent = null;
            Action<E> onApplied = e => appliedEvent = e;
            var ar = new AR(onApplied);
            ar.Method();
            appliedEvent.Should().Not.Be.Null();
        }

        [Test]
        public void NewEventsAreExposedThroughGetUncommittedChanged()
        {
            E newEvent = null;
            Action<E> onApplied = e => newEvent = e;
            var ar = new AR(onApplied);
            ar.Method();
            ar.GetUncommittedChanges().Should().Have.SameSequenceAs(new[] {newEvent});
        }

        [Test]
        public void HistoricalEventsAreAppliedToTheAggregate()
        {
            E historicalEvent = new E();
            E appliedEvent = null;
            Action<E> onApplied = e => appliedEvent = e;
            var ar = new AR(onApplied);
            ar.LoadsFromHistory(new[] {historicalEvent});
            appliedEvent.Should().Be.EqualTo(historicalEvent);
        }

        [Test]
        public void HistoricalEventsAreNotExposedThroughGetUncommitedChanged()
        {
            E historicalEvent = new E();
            Action<E> onApplied = e => { };
            var ar = new AR(onApplied);
            ar.LoadsFromHistory(new[] { historicalEvent });
            ar.GetUncommittedChanges().Should().Be.Empty();
        }

        [Test]
        public void UncommittedEventsAreClearedWhenMarkedAsCommitted()
        {
            Action<E> onApplied = e => { };
            var ar = new AR(onApplied);
            ar.Method();
            ar.MarkChangesAsCommitted();
            ar.GetUncommittedChanges().Should().Be.Empty();
        }

        [Test]
        public void UncommittedEventsAreInOrder()
        {
            var events = new List<E>();
            Action<E> onApply = e => events.Add(e);
            var ar = new AR(onApply);
            ar.Method();
            ar.Method();
            ar.GetUncommittedChanges().Should().Have.SameSequenceAs(events);
        }

        [Test]
        public void LoadFromHistorySetsVersion()
        {
            var history = new Event[] {new E() {Version = 0}, new E() {Version = 1}, new E() {Version = 2}};

            Action<E> onApplied = e => { };
            var ar = new AR(onApplied);

            ar.LoadsFromHistory(history);
            ar.Version.Should().Be.EqualTo(2);
        }

        public class AR : EventSource
        {
            private readonly Action<E> _onApply;

            public AR(Action<E> onApply)
            {
                _onApply = onApply;
            }

            public void Method()
            {
                var e = new E();
                ApplyChange(e);
            }

            private void Apply(E e)
            {
                _onApply(e);
            }
        }

        public class E : Event
        {
        }

    }
}
