using System;
using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;
using Cqrs.Eventing;
using Ninject;

namespace Cqrs.Commanding
{
    public class NinjectCommandSender : ICommandSender 
    {
        private readonly ConcurrentDictionary<Type, Action<Command>> _mapping;

        private readonly IKernel _kernel;

        public NinjectCommandSender(IKernel kernel)
        {
            _kernel = kernel;
            _mapping = new ConcurrentDictionary<Type, Action<Command>>();
        }

       
        public void Send(Command command)
        {
            var commandType = command.GetType();
            var handler = _mapping.GetOrAdd(commandType, BuildHandlerAction);
            handler(command);
        }

        private Action<Command> BuildHandlerAction(Type commandType)
        {
            var mi = GetType()
                .GetMethod("GenericSend", BindingFlags.NonPublic | BindingFlags.Instance)
                .MakeGenericMethod(commandType);


            var param = Expression.Parameter(typeof(Command), "command");

            var conversion = Expression.Convert(param, commandType);

            var instance = Expression.Constant(this);
            var call = Expression.Call(instance, "GenericSend", new Type[] { commandType }, conversion);
            var lamda = Expression.Lambda<Action<Command>>(call, param);
            return lamda.Compile();
        }

        private void GenericSend<TCommand>(TCommand command)
            where TCommand : Command
        {
            var handler = _kernel.TryGet<IHandle<TCommand>>();
            if (handler == null)
                throw new CommandNotHandledException(typeof(TCommand));
            handler.Handle(command);
        }

    }
}
