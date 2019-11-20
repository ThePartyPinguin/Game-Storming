using System;
using System.Collections.Generic;
using System.Text;

namespace GameFrame.Networking.Exception
{
    public sealed class NotSetupCorrectlyException : System.Exception
    {
        public override string Message { get; }

        public NotSetupCorrectlyException(string message)
        {
            Message = message;
        }
    }
}
