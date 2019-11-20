using System;
using System.Collections.Generic;
using System.Text;

namespace GameFrame.Networking.Exception
{
    public sealed class AlreadySetupExcpetion : System.Exception
    {
        public override string Message { get; }

        public AlreadySetupExcpetion(string message)
        {
            Message = message;
        }
    }
}
