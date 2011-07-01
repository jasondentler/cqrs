using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using log4net;

namespace Cqrs.Commanding
{
    public class CommandSender : ICommandSender
    {

        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly Func<Type, dynamic> _createCommandHandler;
        private readonly ConcurrentDictionary<Type, Type> _handlerRegistry;

        public CommandSender(
            Func<Type, object> createCommandHandler)
        {
            _createCommandHandler = createCommandHandler;
            _handlerRegistry = new ConcurrentDictionary<Type, Type>();
        }

        public void RegisterHandlers()
        {
            Log.Debug("Searching app domain for command handlers.");
            var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => !a.GetName().Name.StartsWith("System"));
            var assemblies = new HashSet<Assembly>();
            GetAssemblies(assemblies, loadedAssemblies);
            RegisterHandlers(assemblies.ToArray());
        }

        private void GetAssemblies(HashSet<Assembly> checkedAssemblies, IEnumerable<Assembly> assemblies)
        {
            foreach (var assembly in assemblies)
                if (checkedAssemblies.Add(assembly))
                    GetAssemblies(checkedAssemblies, assembly.GetReferencedAssemblies());
        }

        private void GetAssemblies(HashSet<Assembly> checkedAssemblies, IEnumerable<AssemblyName> assemblyNames )
        {
            GetAssemblies(checkedAssemblies,
                          assemblyNames
                              .Where(an => !an.Name.StartsWith("System"))
                              .Select(Assembly.Load));
        }

        public void RegisterHandlers(params Assembly[] assemblies)
        {
            foreach (var assembly in assemblies)
                RegisterHandlers(assembly);
        }

        public void RegisterHandlers(Assembly assembly)
        {
            Log.DebugFormat("Searching assembly {0} for command handlers.", assembly.GetName().Name);
            RegisterHandlers(assembly.GetTypes());
        }

        public void RegisterHandlers(params Type[] handlerTypes)
        {
            foreach (var handlerType in handlerTypes)
                RegisterHandler(handlerType);
        }

        public void RegisterHandler(Type handlerType)
        {
            Log.DebugFormat("Searching type {0} for command handlers.", handlerType);
            
            var handledCommandTypes = handlerType
                .GetInterfaces()
                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof (IHandle<>))
                .Select(i => i.GetGenericArguments().Single())
                .ToArray();

            foreach (var handledCommandType in handledCommandTypes)
            {
                Log.DebugFormat("Registering {0} to handle {1}", handlerType, handledCommandType);
                if (!_handlerRegistry.TryAdd(handledCommandType, handlerType))
                    throw new DuplicateCommandHandlerFoundException(
                        handledCommandType, _handlerRegistry[handledCommandType], handlerType);
            }
        }

        public void Send<TCommand>(TCommand command) where TCommand : Command
        {
            var commandType = typeof (TCommand);
            Type handlerType;
            if (!_handlerRegistry.TryGetValue(commandType, out handlerType))
                throw new CommandNotHandledException(typeof (TCommand));
            IHandle<TCommand> handler = _createCommandHandler(handlerType);
            handler.Handle(command);
        }

    }
}
