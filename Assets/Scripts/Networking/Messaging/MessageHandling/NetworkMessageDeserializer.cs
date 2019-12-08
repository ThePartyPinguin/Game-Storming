using GameFrame.Networking.Exception;
using GameFrame.Networking.NetworkConnector;
using GameFrame.Networking.Serialization;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using GameFrame.Networking.Messaging.Message;

namespace GameFrame.Networking.Messaging.MessageHandling
{
    public sealed class NetworkMessageDeserializer<TEnum> where TEnum : Enum
    {
        private readonly Queue<byte[]> _messagesToHandleQueue;
        private readonly object _isRunningLock;
        private Task _messageHandlingTask;
        private bool _taskRunning;

        private INetworkMessageSerializer<TEnum> _networkMessageSerializer;
        private readonly NetworkConnector<TEnum> _networkConnector;
        private Dictionary<byte, TEnum> _byteEnumValues;

        private NetworkEventCallbackDatabase<TEnum> _callbackDatabase;

        #region Init

        public NetworkMessageDeserializer(NetworkConnector<TEnum> connector, INetworkMessageSerializer<TEnum> serializer)
        {
            _networkConnector = connector;
            _networkMessageSerializer = serializer;

            _callbackDatabase = NetworkEventCallbackDatabase<TEnum>.Instance;
            SetupByteDictionary();

            _messagesToHandleQueue = new Queue<byte[]>();

            _isRunningLock = new object();
        }

        /// <summary>
        /// Setup a dictionary that contains the enum value and the byte value representing the enum value
        /// </summary>
        private void SetupByteDictionary()
        {
            _byteEnumValues = new Dictionary<byte, TEnum>();

            var enumTypeValues = Enum.GetValues(typeof(TEnum));

            for (int i = 0; i < enumTypeValues.Length; i++)
            {
                var value = enumTypeValues.GetValue(i);
                //TEnum e = GetEnumValue(value);
                _byteEnumValues.Add((byte)Array.IndexOf(enumTypeValues, value), (TEnum)value);
            }
        }

        private TEnum GetEnumValue(object value)
        {
            return (TEnum) Enum.Parse(typeof(TEnum), value.ToString());
        }

        #endregion

        #region MessageHandling

        /// <summary>
        /// Adding a new byte array to the queue so the deserialization task can handle it further, when the task hasn't been started yet, it gets started
        /// </summary>
        /// <param name="dataBuffer">The received bytes from the network</param>
        public void AddMessageToQueue(byte[] dataBuffer)
        {
            _messagesToHandleQueue.Enqueue(dataBuffer);

            lock (_isRunningLock)
            {
                if (!_taskRunning)
                {
                    _messageHandlingTask = new Task(Run);
                    _messageHandlingTask.Start();
                }
            }
        }

        /// <summary>
        /// Check to see if the dataBuffer queue has any entries
        /// </summary>
        /// <returns>'True' if the queue contains entries</returns>
        public bool QueueContainsMessages()
        {
            return _messagesToHandleQueue.Count > 0;
        }

        /// <summary>
        /// Run method for the messageHandlingTask
        /// </summary>
        private void Run()
        {
            _taskRunning = true;
            while (QueueContainsMessages())
            {
                byte[] messageData = _messagesToHandleQueue.Dequeue();
                
                DeserializeAndHandleMessage(messageData);
            }
            _taskRunning = false;
        }

        #endregion

        #region SerializationHandling

        /// <summary>
        /// Converts the byte[] data to the messageEventTypeEnum and the dataBuffer, it get the correct type from the memory database that contains all the messageEventTypes
        /// </summary>
        /// <param name="data">dataBuffer byte[]</param>
        private void DeserializeAndHandleMessage(byte[] data)
        {
            Console.WriteLine("Byte 0: " + data[0]);
            Console.WriteLine("Byte 3: " + data[3]);
            try
            {
                if (ContainsMultipleMessages(data))
                {
                    Console.WriteLine("Data contains multiple messages");
                    HandleMultipleMessages(data);
                }
                else
                {
                    HandleSingleMessage(data);
                }
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e);
            }
        }

        /// <summary>
        /// Handle a buffer containing multiple messages
        /// </summary>
        /// <param name="data">incoming buffer containing multiple messages</param>
        private void HandleMultipleMessages(byte[] data)
        {
            int startIndex = 0;

            while (startIndex < data.Length)
            {
                try
                {
                    Console.WriteLine("startIndex: " + startIndex);
                    if (data[startIndex] != 255 || data[startIndex + 3] != 255)
                        throw new InvalidMessageDataFormatException("Buffer does not have the right format byte 0 and byte 3 should be 0");

                    Console.WriteLine("Byte " + (startIndex) + ':' + data[startIndex]);
                    Console.WriteLine("Byte " + (startIndex + 1) + ':' + data[startIndex + 1]);
                    Console.WriteLine("Byte " + (startIndex + 2) + ':' + data[startIndex + 2]);
                    Console.WriteLine("Byte " + (startIndex + 3) + ':' + data[startIndex + 3]);

                    ushort payloadSize = GetPacketPayloadCount(data, startIndex + 1);

                    DeserializeMessage(data, startIndex + 4, payloadSize);

                    startIndex += payloadSize + 5;
                    Console.WriteLine(startIndex);
                }
                catch (System.Exception e)
                {
                    Console.WriteLine(e);
                    break;
                }
            }

            Console.WriteLine("Parse all messages from buffer");
        }
        /// <summary>
        /// Handle a buffer that only contains a single dataBuffer
        /// </summary>
        /// <param name="data">incoming buffer containing the dataBuffer</param>
        private void HandleSingleMessage(byte[] data)
        {
            try
            {
                if (data[0] != 255 || data[3] != 255)
                    throw new InvalidMessageDataFormatException("Buffer does not have the right format byte 0 and byte 3 should be 0");

                ushort payloadSize = GetPacketPayloadCount(data, 1);
                Console.WriteLine("payloadSize: " + payloadSize);
                DeserializeMessage(data, 4, payloadSize);
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e);
            }
        }

        /// <summary>
        /// Deserializes a dataBuffer from the incoming buffer, with an offset
        /// </summary>
        /// <param name="data">incoming buffer</param>
        /// <param name="startIndex">start position for the deserializer</param>
        /// <param name="payloadSize">length of the payload inside in the data array</param>
        private void DeserializeMessage(byte[] data, int startIndex, int payloadSize)
        {
            Console.WriteLine(data[startIndex]);
            if (!_byteEnumValues.ContainsKey(data[startIndex]))
                throw new MessageEventTypeNotValid("The received messageEventType identifier: " + data[startIndex] + " could not be found in the given typeParameter enum: " + typeof(TEnum));
            
            var messageEventType = _byteEnumValues[data[startIndex]];
            Console.WriteLine(messageEventType + "      " + data[startIndex]);
            var wrapper = _callbackDatabase.GetCallbackWrapper(messageEventType);

            var type = wrapper.MessageType;
            var message = _networkMessageSerializer.DeSerializeWithOffset(data, startIndex + 1, payloadSize, type);

            HandleMessage(message, wrapper);
        }

        /// <summary>
        /// Call the callback of the dataBuffer after deserialization
        /// </summary>
        /// <param name="message">deserialized dataBuffer</param>
        /// <param name="wrapper">Wrapper from the NetworkEventCallbackDatabase database</param>
        private void HandleMessage(NetworkMessage<TEnum> message, NetworkMessageCallbackWrapper<TEnum> wrapper)
        {
            if (_networkConnector != null)
            {
                wrapper.Callback.Invoke(message, _networkConnector.Guid);
            }
            else
            {
                wrapper.Callback.Invoke(message, Guid.Empty);
            }
        }


        /// <summary>
        /// Check if the buffer contains multiple messages
        /// </summary>
        /// <param name="bytes">incoming buffer</param>
        /// <returns>Returns true if the buffer contains multiple messages</returns>
        private bool ContainsMultipleMessages(byte[] bytes)
        {
            return GetPacketPayloadCount(bytes, 1) + 5 < bytes.Length;
        }

        /// <summary>
        /// Check the payload size based on the syntax show in the JsonNetworkMessageSerializer
        /// </summary>
        /// <param name="data">Incoming buffer</param>
        /// <param name="startIndex">Index of the first byte of the ushort payload size identifier</param>
        /// <returns>Returns payload length</returns>
        private ushort GetPacketPayloadCount(byte[] data, int startIndex)
        {
            return BitConverter.ToUInt16(data, startIndex);
        }



        #endregion
    }
}
