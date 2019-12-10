using System;
using System.Collections.Generic;
using System.Text;

namespace GameFrame.Networking.Exception
{
    class InvalidMessageDataFormatException : System.Exception
    {
        public override string Message { get; }

        public InvalidMessageDataFormatException(string message)
        {
            Message = message;
        }
    }
}
