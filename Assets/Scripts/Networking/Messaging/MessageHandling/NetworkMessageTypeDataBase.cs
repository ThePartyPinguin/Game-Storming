using System;
using System.Collections.Generic;
using System.Text;
using GameFrame.Networking.Exception;
using GameFrame.Networking.Messaging.Message;

namespace GameFrame.Networking.Messaging.MessageHandling
{
    public class NetworkMessageTypeDataBase<TEnum, TMessage> : MemoryDatabase<TEnum, Type, NetworkMessageTypeDataBase<TEnum, TMessage>> where TEnum : Enum where TMessage : NetworkMessage<TEnum>
    {
        public void RegisterType(TEnum key, Type type)
        {
            if(KeyExists(key))
                throw new MessageEventAlreadyRegisteredException("Message event: " + key + " has already been registered");

            AddNewValue(key, type);
        }

        public Type GetTypeForKey(TEnum key)
        {
            if(!KeyExists(key))
                throw new MessageEventNotRegisteredException("Message event: " + key + " has not been registered yet");

            return GetValue(key);
        }

        public void Clear()
        {
            ClearDatabase();
        }

        public void Remove(TEnum key)
        {
            RemoveKey(key);
        }
    }
}
