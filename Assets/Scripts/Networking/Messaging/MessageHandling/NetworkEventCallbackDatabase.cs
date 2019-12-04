using GameFrame.Networking.Exception;
using System;
using System.Collections.Generic;
using GameFrame.Networking.Messaging.Message;
using GameFrame.Networking.NetworkConnector;

namespace GameFrame.Networking.Messaging.MessageHandling
{
    public class NetworkEventCallbackDatabase<TEnum> : MemoryDatabase<TEnum, NetworkMessageCallbackWrapper<TEnum>, NetworkEventCallbackDatabase<TEnum>> where TEnum : Enum
    {
        /// <summary>
        /// Register a new NetworkMessageCallbackWrapper, 
        /// </summary>
        /// <typeparam name="TMessage">MessageType to use as reference type for deserializing</typeparam>
        /// <param name="messageEventType">Event used to find the callback on receiving a message with the callback</param>
        /// <param name="callback">The callback for when a message is received</param>

        public void RegisterCallBack<TMessage>(TEnum messageEventType, Action<TMessage, Guid> callback) where TMessage : NetworkMessage<TEnum>
        {
            if (KeyExists(messageEventType))
                throw new MessageEventAlreadyRegisteredException("Messagetype: " + messageEventType + " has already been registered in database: " + this.GetType());


            var action = new Action<NetworkMessage<TEnum>, Guid>((message, connectorId) => callback.Invoke((TMessage) message, connectorId));

            var wrapper = new NetworkMessageCallbackWrapper<TEnum>(typeof(TMessage), action);

            AddNewValue(messageEventType, wrapper);
        }

        public Action<TMessage, Guid> GetCallback<TMessage>(TEnum messageEventType) where TMessage : NetworkMessage<TEnum>
        {
            var wrapper = GetCallbackWrapper(messageEventType);

            if (wrapper.MessageType != typeof(TMessage))
                throw new CallBackTypeNotCorrectException("The requested callback for event: " + messageEventType + " does not match the parameter type: " + typeof(TMessage));

            return wrapper.Callback;
        }

        public NetworkMessageCallbackWrapper<TEnum> GetCallbackWrapper(TEnum messageEventType)
        {
            if (!KeyExists(messageEventType))
                throw new MessageEventNotRegisteredException("Messagetype: " + messageEventType + " has not been registered in MessageHandler: " + this.GetType() + ". Please register this MessageEventType with: RegisterCallback(MessageEventType, INetworkMessageCallback<T>)");

            return GetValue(messageEventType);
        }

        public bool CallbackExists(TEnum messageEventType)
        {
            return KeyExists(messageEventType);
        }

        public void UnRegisterCallback(TEnum messageEventType)
        {
            if (!KeyExists(messageEventType))
                throw new MessageEventNotRegisteredException("Messagetype: " + messageEventType + " has not been registered in database: " + this.GetType());

            RemoveKey(messageEventType);
        }

        public void UnRegisterAllCallbacks()
        {
            ClearDatabase();
        }
    }
}
