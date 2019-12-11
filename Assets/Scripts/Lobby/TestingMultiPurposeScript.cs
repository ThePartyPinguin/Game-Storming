using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingMultiPurposeScript : MonoBehaviour
{
    void Start()
    {
        Debug.Log("Here I am, rockin");
        List<Participant> pp = LobbyManager.GetParticipants();
        foreach(Participant p in pp)
        {
            Debug.Log(p.GetId());
        }
    }
}
