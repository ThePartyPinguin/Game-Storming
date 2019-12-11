using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;


public class LobbyManager : MonoBehaviour
{
    private bool newPlayerJoined = false;
    private float timer = 0;
    private GameObject[] newJoinsBlocks;

    static private List<Participant> participantsConnected;
    private Dictionary<Guid, Button> participantButtons;
    private Dictionary<Guid, GameObject> participantBlocks;
    private List<Color> playerColors;

    public Button btnadd;

    [SerializeField]
    GameObject participantBlock;

    Guid lastGuid;
    
    void Start()
    {
        newJoinsBlocks = new GameObject[10];
        participantsConnected = new List<Participant>();
        participantButtons = new Dictionary<Guid, Button>();
        participantBlocks = new Dictionary<Guid, GameObject>();
        playerColors = new List<Color>();

        DeterminePossibleColors();
    }

    void Update()
    {
        timer += 0.1f;
        if(timer > 1 && newJoinsBlocks.Length != 0)
        {
            timer = 0;

            
        }
    }

    public void TestConnect()
    {
        Guid g = Guid.NewGuid();
        OnPlayerConnect(g, "yes");
        Debug.Log(participantsConnected.Count);
    }

    public void TestDisconnect()
    {
        OnPlayerDisconnect(lastGuid);
        Debug.Log(participantsConnected.Count);
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
        //get new unique color for the player
        Color playerColor = AssignColor();

        Participant newPart = new Participant(playerId, playerName, playerColor);

        participantBlock.GetComponent<SpriteRenderer>().color = playerColor;
        participantBlock.GetComponentInChildren<TextMeshPro>().text = playerName;
        GameObject newPartBlock = Instantiate(participantBlock, transform.position, Quaternion.identity);
        participantBlocks.Add(playerId, newPartBlock);

        participantsConnected.Add(newPart);

        lastGuid = playerId;
    }
    private void SpawnParticipantBlock()
    {

    }
    private void RemoveParicipant(Guid playerId)
    {
        foreach (Participant p in participantsConnected.ToList())
        {
            if (p.GetId() == playerId)
            {
                participantsConnected.Remove(p);
            }
        }
        Destroy(participantBlocks[playerId].gameObject);
        participantBlocks.Remove(playerId);
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

    public void LoadGame(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    static public List<Participant> GetParticipants()
    {
        return participantsConnected;
    }

    static public void ClearParticipantList()
    {
        participantsConnected.Clear();
    }
}
