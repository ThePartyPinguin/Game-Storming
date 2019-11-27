using System;
using GameFrame.Networking.Messaging.Message;

internal interface INetworkMessageSerializer<TEnum> where TEnum : Enum
{
    byte[] Serialize(NetworkMessage<TEnum> message);

    TMessage DeSerializeWithOffset<TMessage>(byte[] data, int offset, int length) where TMessage : NetworkMessage<TEnum>;

    NetworkMessage<TEnum> DeSerializeWithOffset(byte[] data, int offset, int length, Type type);
}
