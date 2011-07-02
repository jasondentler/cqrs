using System;
using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;
using Ninject;

namespace Cqrs.Eventing
{
    public class NinjectEventPublisher : IEventPublisher
    {

        private readonly ConcurrentDictionary<Type, Action<Event>> _mapping;

        private readonly IKernel _kernel;

        public NinjectEventPublisher(IKernel kernel)
        {
            _kernel = kernel;
            _mapping = new ConcurrentDictionary<Type, Action<Event>>();
        }

        public void Publish(Event @event)
        {
            var eventType = @event.GetType();
            var handler = _mapping.GetOrAdd(eventType, BuildHandlerAction);
            handler(@event);
        }

        private Action<Event> BuildHandlerAction(Type eventType)
        {
            var mi = GetType()
                .GetMethod("GenericPublish", BindingFlags.NonPublic | BindingFlags.Instance)
                .MakeGenericMethod(eventType);


            var param = Expression.Parameter(typeof (Event), "event");

            var conversion = Expression.Convert(param, eventType);

            var instance = Expression.Constant(this);
            var call = Expression.Call(instance, "GenericPublish", new Type[] {eventType}, conversion);
            var lamda = Expression.Lambda<Action<Event>>(call, param);
            return lamda.Compile();
        }

        private void GenericPublish<TEvent>(TEvent @event)
            where TEvent : Event
        {
            var handlers = _kernel.GetAll<IHandle<TEvent>>();
            foreach (var handler in handlers)
                handler.Handle(@event);
        }
        
    }

}
