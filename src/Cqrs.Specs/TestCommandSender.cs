using Cqrs.Commanding;

namespace Cqrs.Specs
{
    public class TestCommandSender : ICommandSender 
    {

        private readonly ICommandSender _commandSender;

        public TestCommandSender(ICommandSender commandSender)
        {
            _commandSender = commandSender;
        }

        public void Send<TCommand>(TCommand command) where TCommand : Command
        {
            WhenHelper.OnCommandSentk(command);
            _commandSender.Send(command);
        }
    }
}
