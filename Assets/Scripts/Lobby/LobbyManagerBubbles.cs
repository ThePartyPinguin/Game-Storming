using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class LobbyManagerBubbles : MonoBehaviour
{
    private List<Participant> participantsConnected;
    private List<Color> playerColors;

    [SerializeField]
    GameObject playerBubble;
    BlockGenerator blockGenerator;

    void Start()
    {
        participantsConnected = new List<Participant>();
        playerColors = new List<Color>();
        blockGenerator = transform.GetComponent<BlockGenerator>();

        DeterminePossibleColors();
    }
    public void TestConnect()
    {
        Guid g = new Guid();
        OnPlayerConnect(g, "yes");
    }

    public void OnPlayerConnect(Guid playerId, string playerName)
    {
        CreateNewParticipant(playerId, playerName);
    }

    public void OnPlayerDisconnect(Guid playerId)
    {
        RemoveParicipant(playerId);
    }

    private void CreateNewParticipant(Guid playerId, string playerName)
    {
        Color playerColor = AssignColor();

        Participant newPart = new Participant(playerId, playerName, playerColor);
        participantsConnected.Add(newPart);

        blockGenerator.SpawnBlock(newPart, playerName);
    }

    private void RemoveParicipant(Guid playerId)
    {
        foreach(Participant p in participantsConnected.ToList())
        {
            if(p.GetId() == playerId)
            {
                participantsConnected.Remove(p);
            }
        }   
    }

    //A set of predefined colors for the players
    private void DeterminePossibleColors()
    {
        playerColors.Add(Color.red);
        playerColors.Add(Color.blue);
        playerColors.Add(Color.green);
        playerColors.Add(Color.yellow);
        playerColors.Add(Color.magenta);
        playerColors.Add(Color.cyan);
        playerColors.Add(Color.gray);
    }

    private Color AssignColor()
    {
        //if the players are more than the predefined colors, new colors will be added
        if (participantsConnected.Count >= playerColors.Count)
        {
            return Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
        }
        else
        {
            int index = participantsConnected.Count;
            return playerColors[index];
        }
    }
}
