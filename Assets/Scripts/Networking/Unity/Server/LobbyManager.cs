using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;


public class LobbyManager : MonoBehaviour
{
    private Vector3 blockSpawnPos;
    [SerializeField]
    private Vector2 spawnPosMinMaxX = default;
    [SerializeField]
    private Vector2 spawnPosMinMaxY = default;
    private float counter = 0;
    private Dictionary<Guid, string> newJoins;

    public List<Participant> participantsConnected;
    private Dictionary<Guid, Button> participantButtons;
    public Dictionary<Guid, GameObject> participantBlocks;
    private List<Color> playerColors;

    public Button btnadd;

    public TutorialManager tutorialManager;


    [SerializeField]
    GameObject participantBlock = default;
    [SerializeField]
    TMP_Dropdown roundDropdown = default;
    [SerializeField]
    UnityEvent tutorialHighlighter = default;

    [SerializeField] 
    private ConnectedPlayers connectedPlayers;

    Guid lastGuid;
    
    void Start()
    {
        blockSpawnPos = transform.position;

        newJoins = new Dictionary<Guid, string>();
        participantsConnected = new List<Participant>();
        participantButtons = new Dictionary<Guid, Button>();
        participantBlocks = new Dictionary<Guid, GameObject>();
        playerColors = new List<Color>();

        DeterminePossibleColors();
    }

    void Update()
    {

        //setting a delay on spawning the participant blocks
        counter += 0.1f;
        if(counter > 3 && newJoins.Count != 0)
        {
            Guid temp = newJoins.Keys.First();
            CreateNewParticipant(temp, newJoins[temp]);
            newJoins.Remove(temp);
            tutorialHighlighter.Invoke(); 

            counter = 0;
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
    }

    public void OnPlayerConnect(Guid playerId, string playerName)
    {
        //CreateNewParticipant(playerId, playerName);
        newJoins.Add(playerId, playerName);
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
        GameObject newPartBlock = Instantiate(participantBlock, GenerateSpawnlocation(), Quaternion.identity);
        participantBlocks.Add(playerId, newPartBlock);
        Debug.Log(participantBlocks.Count);
        ColorChanger c = newPartBlock.GetComponent<ColorChanger>();
        c.GetColor(playerColor);

        tutorialManager.objectFloating.Add(newPartBlock.GetComponent<Block>());

        participantsConnected.Add(newPart);


        lastGuid = playerId;
        if (participantsConnected.Count == 6)
        {
            blockSpawnPos.x += participantBlock.GetComponent<BoxCollider2D>().size.x * 2;
            return;
        }
        if (participantsConnected.Count == 12)
        {
            blockSpawnPos.x -= participantBlock.GetComponent<BoxCollider2D>().size.x * 4;
            return;
        }

        tutorialManager.SetPopVisible();
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
        playerColors.Add(new Color32(255, 60, 0,255));
        playerColors.Add(new Color32(128, 0, 128, 255));
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

    public void StartGame(string sceneName)
    {
        if (participantsConnected.Count != UnityServerNetworkManager.Instance.GameServer.ConnectedCount)
        {
            Debug.Log("Not all connected players submitted their name...");
            return;
        }
        UnityServerNetworkManager.Instance.GetComponent<GameEvents>().SendStartGameEvent();
        SceneManager.LoadScene(sceneName);
    }

    void OnDestroy()
    {
        //passing nesessary info to StaticGameInfo before destroying the Lobby
        StaticGameInfo.participants = participantsConnected;
        StaticGameInfo.rounds = Convert.ToInt32(roundDropdown.options[roundDropdown.value].text);
    }

    // fuckin stole this from Marko's BlockGenerator class lmao
    private Vector2 GenerateSpawnlocation()
    {
        if (spawnPosMinMaxX.x < spawnPosMinMaxX.y && spawnPosMinMaxY.x < spawnPosMinMaxY.y)
        {
            return new Vector2(transform.position.x + Random.Range(spawnPosMinMaxX.x, spawnPosMinMaxX.y), Random.Range(spawnPosMinMaxY.x, spawnPosMinMaxY.y));
        }
        Debug.LogError("[BlockGenerator.GenerateSpawnLocation] : Error generating spawn location. Defaulting to zero.");
        return Vector2.zero;
    }
}
