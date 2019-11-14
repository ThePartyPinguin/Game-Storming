using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface INetworkMessageCallback<T> where T : NetworkMessage
{
    void Call(T message);
}
