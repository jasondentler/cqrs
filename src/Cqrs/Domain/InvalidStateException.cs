using System;

namespace Cqrs.Domain
{
    public class InvalidStateException : ApplicationException
    {

        public InvalidStateException(string message)
            : base(message)
        {
        }

    }
}
