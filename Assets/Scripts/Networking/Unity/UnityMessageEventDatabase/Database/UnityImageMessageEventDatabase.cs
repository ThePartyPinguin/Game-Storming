using UnityEngine;
using System;
using UnityEngine.Events;

public class UnityImageMessageEventDatabase : UnityBaseMessageEventsDatabase<ImageNetworkMessage, UnityImageMessageEventDatabase.ImageCallbackWrapper, UnityImageMessageEventDatabase.ImageMessageCallback>
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