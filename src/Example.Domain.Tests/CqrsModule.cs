using Cqrs.Commanding;
using Cqrs.Domain;
using Cqrs.EventStore;
using Cqrs.EventStore.Memory;
using Cqrs.Eventing;
using Cqrs.Specs;
using Example.Menu;
using Ninject;
using Ninject.Modules;

namespace Example
{
    public class CqrsModule : NinjectModule 
    {
        public override void Load()
        {
            Kernel.Bind(typeof (IRepository<>))
                .To(typeof (TestRepository<>))
                .InSingletonScope();

            Kernel.Bind<IEventStore>()
                .To<MemoryEventStore>()
                .InSingletonScope();

            Kernel.Bind<IEventPublisher>()
                .To<NullEventPublisher>()
                .InSingletonScope();

            SetupCommandSender();
        }

        private void SetupCommandSender()
        {

            var commandSender = new CommandSender(handler => Kernel.Get(handler));
            commandSender.RegisterHandlers(typeof (ItemCommandHandler).Assembly);

            var testSender = new TestCommandSender(commandSender);

            var queuedSender = new QueuedCommandSender(testSender);

            Kernel.Bind<ICommandSender>()
                .ToConstant(queuedSender);
        }

    }
}
