using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class IdeaMessageCallbacks : MonoBehaviour
{
    public float HorizontalSpawnOffset;
    public float VerticalSpawnOffset;
    public GameObject ObjectToSpawn;
    
    public void ClientSendIdea(IdeaNetworkMessage message, Guid clientId)
    {
        Debug.Log(clientId);
        Debug.Log(message.Idea);

        var o = Instantiate(ObjectToSpawn);

        o.name = message.Idea;

        Vector3 currentScale = ObjectToSpawn.transform.lossyScale;

        o.transform.localScale = new Vector3(message.Idea.Length * 0.5f, currentScale.y, currentScale.z);

        float x = Random.Range(-HorizontalSpawnOffset, HorizontalSpawnOffset);
        float y = VerticalSpawnOffset;

        Vector3 position = new Vector3(x, y, 0);

        o.transform.position = position;

        Destroy(o, 5f);

    }
}
