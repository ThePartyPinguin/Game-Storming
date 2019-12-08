using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdeaNetworkMessage : BaseNetworkMessage
{
    public string Idea { get; }

    public IdeaNetworkMessage(NetworkEvent messageEventType, string idea) : base(messageEventType)
    {
        Idea = idea;
    }
}
