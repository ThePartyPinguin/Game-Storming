using System;
using System.Collections.Generic;
using System.Text;

namespace GameFrame.Networking.Exception
{
    public sealed class InvalidIPAdressException : System.Exception
    {
        public override string Message { get; }

        public InvalidIPAdressException(string message)
        {
            Message = message;
        }
    }
}
