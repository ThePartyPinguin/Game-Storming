using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameFrame.Networking.Messaging
{
    enum InternalNetworkEvent
    {
        //Events send by client
        CLIENT_TO_SERVER_HANDSHAKE,

        //Events send by server
        SERVER_TO_CLIENT_HANDSHAKE
    }
}
