using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class MessageTypeAlreadyRegisteredException : Exception
{
    public override string Message { get; }

    public MessageTypeAlreadyRegisteredException(string message)
    {
        Message = message;
    }
}
