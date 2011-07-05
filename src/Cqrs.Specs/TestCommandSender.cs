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

        public void Send(Command command)
        {
            WhenHelper.OnCommandSentk(command);
            _commandSender.Send(command);
        }
    }
}
