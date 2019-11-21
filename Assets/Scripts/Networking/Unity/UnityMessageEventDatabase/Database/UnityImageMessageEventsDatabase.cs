using UnityEngine;
using System;
using UnityEngine.Events;

public class UnityImageMessageEventsDatabase : UnityBaseMessageEventsDatabase<ImageNetworkMessage, UnityImageMessageEventsDatabase.ImageCallbackWrapper, UnityImageMessageEventsDatabase.ImageMessageCallback>
{
    [Serializable]
    public class ImageCallbackWrapper : BaseMessageCallbackWrapper<ImageNetworkMessage, ImageMessageCallback>
    {
    }

    [Serializable]
    public class ImageMessageCallback : BaseMessageCallback<ImageNetworkMessage>
    {

    }
}