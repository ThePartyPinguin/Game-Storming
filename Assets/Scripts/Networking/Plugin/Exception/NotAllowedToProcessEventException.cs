using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameFrame.Networking.Exception
{
    class NotAllowedToProcessEventException : System.Exception
    {
        public override string Message { get; }

        public NotAllowedToProcessEventException(string message)
        {
            Message = message;
        }
    }
}
