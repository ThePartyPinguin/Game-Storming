using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISerializer
{
    byte[] Serialize(NetworkMessage message);

    NetworkMessage DeSerialize(byte[] data);

    NetworkMessage DeSerializeWithOffset(byte[] data, int offset, int length);
}
