using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class DataNetworkMessage : NetworkMessage
{
    public object data { get; }
    public DataNetworkMessage(MessageType messageType, object data) : base(messageType)
    {
        this.data = data;
    }
}
