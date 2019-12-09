using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;


public class LobbyManager : MonoBehaviour
{
    private List<Participant> participantsConnected;

    [SerializeField]
    Text txtInstruction;
    [SerializeField]
    Image playersGrid;
    [SerializeField]
    Button playerBtn;
    Text btnTextPlayerName;

    List<Color> playerColors;
    
    public void Start()
    {
        btnTextPlayerName = playerBtn.GetComponentInChildren<Text>();

        participantsConnected = new List<Participant>();
        playerColors = new List<Color>();

        DeterminePossibleColors();


        //for (int i = 0; i < 10; i++)
            
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

    private void CreateNewParticipant(Guid playerId, string playerName)
    {
        //get new unique color for the player
        Color playerColor = AssignColor();

        //button setting - name and color 
        //colors in Button seems to not be a reference, so playerBtn.colors.normalColor = playerColor does not work.
        btnTextPlayerName.text = playerName;
        ColorBlock cb = playerBtn.colors;
        cb.normalColor = playerColor;
        playerBtn.colors = cb;

        Instantiate(playerBtn, playersGrid.transform);

        Participant newPart = new Participant(playerId, playerName, playerColor);
        participantsConnected.Add(newPart);
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
        if(participantsConnected.Count >= playerColors.Count)
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
