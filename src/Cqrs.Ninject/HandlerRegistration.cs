using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Ninject;
using log4net;

namespace Cqrs
{
    public class HandlerRegistration
    {

        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly IKernel _kernel;

        public HandlerRegistration(IKernel kernel)
        {
            _kernel = kernel;
        }

        public HandlerRegistration RegisterHandlers(params Assembly[] assemblies)
        {
            foreach (var assembly in assemblies)
                RegisterHandlers(assembly);
            return this;
        }

        public HandlerRegistration RegisterHandlers(Assembly assembly)
        {
            Log.DebugFormat("Searching assembly {0} for message handlers.", assembly.GetName().Name);
            RegisterHandlers(assembly.GetTypes());
            return this;
        }

        public HandlerRegistration RegisterHandlers(params Type[] handlerTypes)
        {
            foreach (var handlerType in handlerTypes)
                RegisterHandler(handlerType);
            return this;
        }

        public HandlerRegistration RegisterHandler(Type handlerType)
        {
            Log.DebugFormat("Searching type {0} for message handlers.", handlerType);

            var handlerMap = handlerType
                .GetInterfaces()
                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof (IHandle<>))
                .Select(i => new
                                 {
                                     messageType = i.GetGenericArguments().Single(),
                                     interfaceType = i
                                 }).ToArray();

            foreach (var item in handlerMap)
            {
                Log.DebugFormat("Registering {0} to handle {1}", handlerType, item.messageType);

                _kernel.Bind(item.interfaceType)
                    .To(handlerType);
            }
            return this;
        }


    }
}
