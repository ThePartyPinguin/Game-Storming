using System;
using System.Collections;
using System.Collections.Generic;
using GameFrame.Networking.Messaging.MessageHandling;
using GameFrame.UnityHelpers.Networking;
using GameFrame.UnityHelpers.Networking.Message;
using UnityEngine;

public class NetworkedGameObjectsFactory : BaseFactory<NetworkedGameObject, NetworkedGameObjectsFactory>
{
    private Dictionary<Guid, NetworkedGameObject> _currentNetworkedGameObjects;
    private Dictionary<Guid, NetworkedGameObject> _tempAllocatedNetworkedGameObjects;

    private Action<NetworkedGameObject> _onProduceCallback;

    public void SetupOnProduceCallback(Action<NetworkedGameObject> onProduceCallback)
    {
        _onProduceCallback = onProduceCallback;
    }
    
    public override void OnProducedNew(ref NetworkedGameObject newObject)
    {
        Guid tempNetworkId = Guid.NewGuid();

        newObject.SetNetworkId(tempNetworkId);

        _tempAllocatedNetworkedGameObjects.Add(tempNetworkId, newObject);

        _onProduceCallback?.Invoke(newObject);
    }

    public void UpdateNetworkObjectNetworkId(Guid oldNetworkId, Guid newNetworkId)
    {
        if (_tempAllocatedNetworkedGameObjects.ContainsKey(oldNetworkId))
        {
            _tempAllocatedNetworkedGameObjects[oldNetworkId].SetNetworkId(newNetworkId);
            _tempAllocatedNetworkedGameObjects.Remove(oldNetworkId);
        }
    }
}
