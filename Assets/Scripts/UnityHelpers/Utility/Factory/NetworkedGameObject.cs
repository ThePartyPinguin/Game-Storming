using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NetworkedGameObject : FactoryInitGameObject<NetworkedGameObject>
{
    public int ObjectTypeId;
    public Guid NetworkId { get; private set; }
    protected sealed override void OnDisabled()
    {
        
    }

    protected sealed override void Init(params object[] args)
    {
        NetworkId = Guid.Empty;
    }

    public void SetNetworkId(Guid networkId)
    {
        NetworkId = networkId;
    }
}
