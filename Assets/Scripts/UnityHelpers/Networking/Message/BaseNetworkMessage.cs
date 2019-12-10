using GameFrame.Networking.Messaging.Message;

namespace GameFrame.UnityHelpers.Networking.Message
{
    public class BaseNetworkMessage : NetworkMessage<NetworkEvent>
    {
        public BaseNetworkMessage(NetworkEvent messageEventType) : base(messageEventType)
        {
        }
    }
}
