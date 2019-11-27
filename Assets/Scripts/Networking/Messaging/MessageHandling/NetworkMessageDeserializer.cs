using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GameFrame.Networking.Exception;
using GameFrame.Networking.Messaging.Message;
using UnityEngine;

namespace GameFrame.Networking.Messaging.MessageHandling
{
    sealed class NetworkMessageDeserializer<TEnum> where TEnum : Enum
    {
        public Action<NetworkMessage<TEnum>> OnMessageHandledCallback { get; private set; }

        private readonly NetworkMessageTypeDataBase<TEnum> _messageTypeDatabase; 
        private readonly Queue<byte[]> _messagesToHandleQueue;
        private readonly object _isRunningLock;
        private readonly Task _messageHandlingTask;
        private bool _taskRunning;

        private INetworkMessageSerializer<TEnum> _networkMessageSerializer;

        private Dictionary<byte, TEnum> _byteEnumValues;

        #region Init

        public NetworkMessageDeserializer(Action<NetworkMessage<TEnum>> onMessageHandledCallback, INetworkMessageSerializer<TEnum> serializer)
        {
            _networkMessageSerializer = serializer;
            OnMessageHandledCallback = onMessageHandledCallback;

            _messageTypeDatabase = NetworkMessageTypeDataBase<TEnum>.Instance;
            SetupByteDictionary();

            _messagesToHandleQueue = new Queue<byte[]>();

            _isRunningLock = new object();

            _messageHandlingTask = new Task(Run);
        }

        public void SetNewOnMessageHandledCallback(Action<NetworkMessage<TEnum>> onMessageHandledCallback)
        {
            OnMessageHandledCallback = onMessageHandledCallback;
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
        /// <param name="message">The received bytes from the network</param>
        public void AddMessageToQueue(byte[] message)
        {
            _messagesToHandleQueue.Enqueue(message);

            lock (_isRunningLock)
            {
                if (!_taskRunning)
                    _messageHandlingTask.Start();
            }
        }

        /// <summary>
        /// Check to see if the message queue has any entries
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

                if (DeserializeMessage(messageData, out var message))
                {
                    CallOnMessageDeserialized(message);
                }
            }
            _taskRunning = false;
        }

        #endregion

        #region SerializationHandling

        /// <summary>
        /// Converts the byte[] data to the messageEventTypeEnum and the message, it get the correct type from the memory database that contains all the messageEventTypes
        /// </summary>
        /// <param name="data">message byte[]</param>
        /// <param name="messageEventType">gives the messageEventType enum value from data[0]</param>
        /// <param name="message">gives back the deserialized message from data</param>
        /// <returns></returns>
        private bool DeserializeMessage(byte[] data, out NetworkMessage<TEnum> message)
        {
            try
            {
                if (!_byteEnumValues.ContainsKey(data[0]))
                    throw new MessageEventTypeNotValid("The received messageEventType identifier: " + data[0] + " could not be found in the given typeParameter enum: " + typeof(TEnum));

                var messageEventType = _byteEnumValues[data[0]];

                Debug.Log(messageEventType);
                var type = _messageTypeDatabase.GetTypeForKey(messageEventType);

                Debug.Log(type);

                message = _networkMessageSerializer.DeSerializeWithOffset(data, 1, data.Length - 1, type);
                return true;
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        #endregion

        #region CallbackHandling

        /// <summary>
        /// After a message has been DeSerialized this method gets called after which the OnMessageHandledCallback gets called
        /// </summary>
        /// <param name="message">The deserialized message</param>
        private void CallOnMessageDeserialized(NetworkMessage<TEnum> message)
        {
            Debug.Log(message.MessageEventType);
            OnMessageHandledCallback?.Invoke(message);
        }

        #endregion
    }
}
