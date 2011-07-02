using Ninject;

namespace Cqrs.Commanding
{
    public class NinjectCommandSender : ICommandSender 
    {
        private readonly IKernel _kernel;

        public NinjectCommandSender(IKernel kernel)
        {
            _kernel = kernel;
        }

        public void Send<TCommand>(TCommand command) where TCommand : Command
        {
            var handler = _kernel.TryGet<IHandle<TCommand>>();
            if (handler == null)
                throw new CommandNotHandledException(typeof (TCommand));
            handler.Handle(command);
        }
    }
}
