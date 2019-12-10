using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameFrame.UnityHelpers.Networking.Message
{
    class ServerInstantiateResponse : BaseNetworkMessage
    {
        public Guid ClientAssignedNetworkId { get; }
        public Guid ServerAssignedNetworkId { get; }

        public ServerInstantiateResponse(NetworkEvent messageEventType, Guid clientAssignedNetworkId, Guid serverAssignedNetworkId) : base(messageEventType)
        {
            ClientAssignedNetworkId = clientAssignedNetworkId;
            ServerAssignedNetworkId = serverAssignedNetworkId;
        }
    }
}
