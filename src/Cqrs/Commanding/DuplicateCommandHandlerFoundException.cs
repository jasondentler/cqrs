using System;

namespace Cqrs.Commanding
{

    public class DuplicateCommandHandlerFoundException : Exception
    {

        private static string GetMessage(Type commandType, Type registeredHandlerType, Type newHandlerType)
        {
            return string.Format("Unable to register command handler {2} because {1} is already registered to handle {0} commands.",
                                 commandType,
                                 registeredHandlerType,
                                 newHandlerType);
        }

        public DuplicateCommandHandlerFoundException(Type commandType, Type registeredHandlerType, Type newHandlerType)
            : base(GetMessage(commandType, registeredHandlerType, newHandlerType))
        {

        }

    }

}
