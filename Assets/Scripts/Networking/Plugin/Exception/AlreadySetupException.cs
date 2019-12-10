using System;
using System.Collections.Generic;
using System.Text;

namespace GameFrame.Networking.Exception
{
    public sealed class AlreadySetupException : System.Exception
    {
        public override string Message { get; }

        public AlreadySetupException(string message)
        {
            Message = message;
        }
    }
}
