using System;
using System.Reflection;
using log4net;

namespace Cqrs.Commanding
{
    public class CommandSender : ICommandSender
    {

        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly Func<Type, dynamic> _getHandler;

        public CommandSender(
            Func<Type, dynamic> getHandler)
        {
            _getHandler = getHandler;
        }

        public void Send<TCommand>(TCommand command) where TCommand : Command
        {
            Log.DebugFormat("Sending {0}", typeof (TCommand));
            var handler = _getHandler(typeof (TCommand));
            handler.Handle(command);
        }

    }
}
