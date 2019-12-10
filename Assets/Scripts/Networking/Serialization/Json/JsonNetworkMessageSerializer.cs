using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using GameFrame.Networking.Messaging.Message;
using Newtonsoft.Json;

namespace GameFrame.Networking.Serialization.Json
{
    public class JsonNetworkMessageSerializer<TEnum> : INetworkMessageSerializer<TEnum> where TEnum : Enum
    {
        private static readonly Dictionary<TEnum, byte> SerializationEnumByteValues;

        #region init

        static JsonNetworkMessageSerializer()
        {
            SerializationEnumByteValues = new Dictionary<TEnum, byte>();

            SetupDictionaries();
        }

        private static void SetupDictionaries()
        {
            var enumTypeValues = Enum.GetValues(typeof(TEnum));

            for (int i = 0; i < enumTypeValues.Length; i++)
            {
                var value = enumTypeValues.GetValue(i);

                byte byteValue = (byte)Array.IndexOf(enumTypeValues, value);

                SerializationEnumByteValues.Add((TEnum)value, byteValue);
            }
        }

        #endregion

        private byte[] EncodeJsonString(string json, TEnum eventType)
        {
            char[] chars = json.ToCharArray();
        
            ushort byteCount = (ushort)Encoding.Unicode.GetByteCount(json);
            
            byte[] bytes = new byte[byteCount + 5];

            var byteCountBytes = BitConverter.GetBytes(byteCount);

            bytes[0] = 255;
            bytes[1] = byteCountBytes[0];
            bytes[2] = byteCountBytes[1];
            bytes[3] = 255;
            bytes[4] = SerializationEnumByteValues[eventType];

            Encoding.Unicode.GetBytes(chars, 0, chars.Length, bytes, 5);

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
            Console.WriteLine(json);

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
}
