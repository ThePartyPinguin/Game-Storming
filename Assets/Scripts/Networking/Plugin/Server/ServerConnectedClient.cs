using System;
using System.Collections.Generic;
using System.Text;
using GameFrame.Networking.Messaging.Message;
using GameFrame.Networking.NetworkConnector;

namespace GameFrame.Networking.Server
{
    public class ServerConnectedClient<TEnum> where TEnum : Enum
    {
        public Guid ClientId { get; }
        private readonly NetworkConnector<TEnum> _networkConnector;

        public ServerConnectedClient(Guid clientId, NetworkConnector<TEnum> networkConnector)
        {
            ClientId = clientId;
            _networkConnector = networkConnector;
        }

        public void SecureSendMessage(NetworkMessage<TEnum> message)
        {
            _networkConnector.SecureSendMessage(message);
        }

        public void SendMessage(NetworkMessage<TEnum> message)
        {
            _networkConnector.SendMessage(message);
        }

        public void Stop()
        {
            _networkConnector?.Stop();
        }

        public void AddAllowedEvent(TEnum allowedEvent)
        {
            _networkConnector.AddAllowedEvent(allowedEvent);
        }

        public void AddAllowedEvent(IEnumerable<TEnum> allowedEvents)
        {
            _networkConnector.AddMultipleAllowedEvents(allowedEvents);
        }
        public void RemoveAllowedEvent(TEnum allowedEvent)
        {
            _networkConnector.RemoveAllowedEvent(allowedEvent);
        }

        public void RemoveAllowedEvent(IEnumerable<TEnum> allowedEvents)
        {
            _networkConnector.RemoveMultipleAllowedEvents(allowedEvents);
        }
    }
}
