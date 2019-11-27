using System;
using System.Collections.Generic;
using System.Text;

namespace GameFrame.Networking.Exception
{
    public sealed class MessageEventTypeNotValid : System.Exception
    {
        public override string Message { get; }

        public MessageEventTypeNotValid(string message)
        {
            Message = message;
        }
    }
}
