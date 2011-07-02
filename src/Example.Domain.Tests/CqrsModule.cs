using Cqrs;
using Cqrs.Commanding;
using Cqrs.Domain;
using Cqrs.EventStore;
using Cqrs.EventStore.Memory;
using Cqrs.Eventing;
using Cqrs.Specs;
using Example.Menu;
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
                .To<NinjectEventPublisher>()
                .InSingletonScope();

            RegisterHandlers();
            SetupCommandSender();
        }

        private void RegisterHandlers()
        {
            new HandlerRegistration(Kernel)
                .RegisterHandlers(typeof (ItemCommandHandler).Assembly)
                .RegisterHandler(typeof (RegisterEventSources));
        }

        private void SetupCommandSender()
        {

            var commandSender = new NinjectCommandSender(Kernel);

            var testSender = new TestCommandSender(commandSender);

            var queuedSender = new QueuedCommandSender(testSender);

            Kernel.Bind<ICommandSender>()
                .ToConstant(queuedSender);

        }

    }
}
