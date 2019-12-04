using System;
using System.Text;
using GameFrame.Networking.Messaging.Message;
using Newtonsoft.Json;

sealed class JsonNetworkMessageSerializer<TEnum> : INetworkMessageSerializer<TEnum> where TEnum : Enum
{
    private byte[] EncodeJsonString(string json, TEnum eventType)
    {
        char[] chars = json.ToCharArray();
        
        int byteCount = Encoding.Unicode.GetByteCount(chars);

        byte[] bytes = new byte[byteCount + 1];

        var enumArray = Enum.GetValues(typeof(TEnum));

        bytes[0] = (byte)Array.IndexOf(enumArray, eventType);

        Encoding.Unicode.GetBytes(chars, 0, chars.Length, bytes, 1);

        return bytes;
    }

    public byte[] Serialize(NetworkMessage<TEnum> message)
    {
        string json = ConvertToJson(message);
        return EncodeJsonString(json, message.MessageEventType);
    }

    public TMessage DeSerializeWithOffset<TMessage>(byte[] data, int offset, int length) where TMessage : NetworkMessage<TEnum>
    {
        string json = DeCodeBytes(data, offset, length);
        return DeSerializeJson<TMessage>(json);
    }

    public NetworkMessage<TEnum> DeSerializeWithOffset(byte[] data, int offset, int length, Type type)
    {
        string json = DeCodeBytes(data, offset, length);
        return DeSerializeJson(json, type);
    }

    private string ConvertToJson(NetworkMessage<TEnum> message)
    {
        return JsonConvert.SerializeObject(message);
    }

    private TMessage DeSerializeJson<TMessage>(string json) where TMessage : NetworkMessage<TEnum>
    {
        return JsonConvert.DeserializeObject<TMessage>(json);
    }

    private NetworkMessage<TEnum> DeSerializeJson(string json, Type type)
    {
        return (NetworkMessage<TEnum>)JsonConvert.DeserializeObject(json, type);
    }

    private string DeCodeBytes(byte[] data, int offset, int count)
    {
        return Encoding.Unicode.GetString(data, offset, count);
    }
}
