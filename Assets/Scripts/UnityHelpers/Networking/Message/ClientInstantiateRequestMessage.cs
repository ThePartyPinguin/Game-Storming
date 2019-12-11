using System;
using System.Collections;
using System.Collections.Generic;
using GameFrame.UnityHelpers.Networking;
using GameFrame.UnityHelpers.Networking.Message;
using UnityEngine;

namespace GameFrame.UnityHelpers.Networking.Message
{
    public class ClientInstantiateRequestMessage : BaseNetworkMessage
    {
        public int NetworkedGameObjectTypeId { get; }
        public Guid TempNetworkId { get; }

        public ClientInstantiateRequestMessage(NetworkEvent messageEventType, int networkedGameObjectTypeId, Guid tempNetworkId) : base(messageEventType)
        {
            NetworkedGameObjectTypeId = networkedGameObjectTypeId;
            TempNetworkId = tempNetworkId;
        }
    }
}

