using System;
using System.Collections.Generic;
using System.Linq;
using Cqrs.Commanding;
using Cqrs.Domain;
using Cqrs.Eventing;
using NUnit.Framework;
using SharpTestsEx;

namespace Cqrs.Sagas
{
    [TestFixture]
    public class SagaTests
    {

        [Test]
        public void NewEventsAreAppliedToTheSaga()
        {
            E appliedEvent = null;
            Action<E> onApplied = e => appliedEvent = e;
            var saga = new MySaga(onApplied);
            saga.Method();
            appliedEvent.Should().Not.Be.Null();
        }

        [Test]
        public void NewEventsAreExposedThroughGetUncommittedChanged()
        {
            E newEvent = null;
            Action<E> onApplied = e => newEvent = e;
            var saga = new MySaga(onApplied);
            saga.Method();
            saga.GetUncommittedChanges().Should().Have.SameSequenceAs(new[] {newEvent});
        }

        [Test]
        public void NewDispatchesAreExposedThroughGetDispatches()
        {
            Action<E> onApplied = e => { };
            var saga = new MySaga(onApplied);
            saga.Method();
            saga.GetDispatches().Count().Should().Be.EqualTo(1);
        }

        [Test]
        public void HistoricalEventsAreAppliedToTheSaga()
        {
            E historicalEvent = null;
            E appliedEvent = null;
            Action<E> onApplied = e => appliedEvent = e;
            var saga = new MySaga(onApplied);
            saga.LoadsFromHistory(new[] {historicalEvent});
            appliedEvent.Should().Be.EqualTo(historicalEvent);
        }

        [Test]
        public void HistoricalEventsAreNotExposedThroughGetUncommitedChanged()
        {
            E historicalEvent = null;
            Action<E> onApplied = e => { };
            var saga = new MySaga(onApplied);
            saga.LoadsFromHistory(new[] { historicalEvent });
            saga.GetUncommittedChanges().Should().Be.Empty();
        }

        [Test]
        public void UncommittedEventsAreClearedWhenMarkedAsCommitted()
        {
            Action<E> onApplied = e => { };
            var saga = new MySaga(onApplied);
            saga.Method();
            saga.MarkChangesAsCommitted();
            saga.GetUncommittedChanges().Should().Be.Empty();
        }

        [Test]
        public void DispatchesAreClearedWhenMarkedAsCommitted()
        {
            Action<E> onApplied = e => { };
            var saga = new MySaga(onApplied);
            saga.Method();
            saga.MarkChangesAsCommitted();
            saga.GetDispatches().Should().Be.Empty();
        }

        [Test]
        public void UncommittedEventsAreInOrder()
        {
            var events = new List<E>();
            Action<E> onApply = e => events.Add(e);
            var saga = new MySaga(onApply);
            saga.Method();
            saga.Method();
            saga.GetUncommittedChanges().Should().Have.SameSequenceAs(events);
        }

        public class MySaga : Saga
        {
            private readonly Action<E> _onApply;

            public MySaga(Action<E> onApply)
            {
                _onApply = onApply;
            }

            public void Method()
            {
                var e = new E();
                ApplyChange(e);
                var c = new C();
                Dispatch(c);
            }

            private void Apply(E e)
            {
                _onApply(e);
            }
        }

        public class E : Event
        {
        }

        public class C : Command
        {
        }

    }
}
