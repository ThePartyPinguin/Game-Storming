using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScriptLobby : MonoBehaviour
{
    void Start()
    {
        Debug.Log(StaticGameInfo.participants.Count + " " + StaticGameInfo.rounds);
    }

 
}
