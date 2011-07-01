using System;

namespace Cqrs.Commanding
{

    public class CommandNotHandledException : Exception
    {

        private static string GetMessage(Type commandType)
        {
            return string.Format("The given command type {0} has no registered handlers.",
                                 commandType);
        }

        public CommandNotHandledException(Type commandType)
            : base(GetMessage(commandType))
        {

        }

    }

}
