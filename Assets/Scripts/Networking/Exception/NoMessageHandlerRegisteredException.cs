using System;
using System.Collections.Generic;
using System.Text;

namespace GameFrame.Networking.Exception
{
    public sealed class NoMessageHandlerRegisteredException : System.Exception
    {
        public override string Message { get; }

        public NoMessageHandlerRegisteredException(string message)
        {
            Message = message;
        }
    }
}
