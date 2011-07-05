using Cqrs;
using Cqrs.Commanding;
using Cqrs.Domain;
using Cqrs.EventStore;
using Cqrs.EventStore.Memory;
using Cqrs.EventStore.MsSql;
using Cqrs.Eventing;
using Cqrs.Specs;
using Example.Menu;
using Example.Services;
using Ninject;
using Ninject.Modules;
using EventDescriptor = Cqrs.EventStore.MsSql.EventDescriptor;

namespace Example
{
    public class CqrsModule : NinjectModule 
    {
        public override void Load()
        {
            Kernel.Bind<IRepository>()
                .To<TestRepository>()
                .InSingletonScope();

            SetupMsSqlEventStore();

            Kernel.Bind<IEventPublisher>()
                .To<NinjectEventPublisher>()
                .InSingletonScope();

            // To share a single instance with the handler registration
            // If we used a db, we could separate the handler from the service
            Kernel.Bind<IProductService>()
                .ToMethod(ctx => ctx.Kernel.Get<ProductService>());

            Kernel.Bind<ProductService>()
                .ToSelf()
                .InSingletonScope();

            RegisterHandlers();
            SetupCommandSender();
        }

        private void RegisterHandlers()
        {
            new HandlerRegistration(Kernel)
                .RegisterHandlers(typeof (ItemCommandHandler).Assembly)
                .RegisterHandlers(typeof(ProductService).Assembly)
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

        private void SetupMsSqlEventStore()
        {
            Kernel.Bind<IEventStore>()
                .To<MsSqlEventStore>()
                .InSingletonScope();

            Kernel.Bind<ITypeNameResolver>()
                .To<SimpleTypeNameResolver>();

            Kernel.Bind<ISerializer<EventDescriptor>>()
                .To<JsonSerializer>();

        }

    }
}
