using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class MessageTypeNotRegisteredException : Exception
{
    public override string Message { get; }

    public MessageTypeNotRegisteredException(string message)
    {
        Message = message;
    }
}
