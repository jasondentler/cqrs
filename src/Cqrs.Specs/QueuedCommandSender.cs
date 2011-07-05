using System;
using System.Collections.Concurrent;
using System.Reflection;
using Cqrs.Commanding;
using log4net;

namespace Cqrs.Specs
{

    /// <summary>
    /// Executes commands synchronously 
    /// </summary>
    /// <remarks>>
    /// This is only intended for test scenarios
    /// </remarks>
    public class QueuedCommandSender : ICommandSender
    {

        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);


        private readonly ICommandSender _commandSender;
        private readonly ConcurrentQueue<Action> _queue;
        private readonly object _sync = new object();
        private bool _isExecuting;

        public QueuedCommandSender(
            ICommandSender commandSender)
        {
            _commandSender = commandSender;
            _queue = new ConcurrentQueue<Action>();
        }

        public void Send(Command command)
        {
            _queue.Enqueue(() => _commandSender.Send(command));
            ExecuteUntilEmpty();
        }

        private void ExecuteUntilEmpty()
        {
            if (!_isExecuting)
            {
                lock (_sync)
                {
                    if (!_isExecuting)
                    {
                        _isExecuting = true;
                        try
                        {
                            SafeExecuteUntilEmpty();
                        }
                        finally
                        {
                            ClearQueue();
                            _isExecuting = false;
                        }
                    }
                }
            }
        }

        private void SafeExecuteUntilEmpty()
        {
            Action action;
            while (_queue.TryDequeue(out action))
                action();
        }

        private void ClearQueue()
        {
            Action action;
            while (_queue.TryDequeue(out action))
            {
            }
        }


    }

}
