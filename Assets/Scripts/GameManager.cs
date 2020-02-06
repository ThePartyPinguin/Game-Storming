using System;
using System.Collections;
using System.Collections.Generic;
using GameFrame.UnityHelpers.Networking;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class GameManager : MonoSingleton<GameManager>
{
    #region fields
    private List<Participant> participants; 
    private int currentBuilderIndex;
    //[SerializeField]
    //private CountdownTimer timer = default;

    [SerializeField]
    private Transform foundationTop = default;
    private GameObject currentDragObject;

    [SerializeField]
    private StringBasedUnityEvent newBuilderCalled = default;

    [SerializeField]
    public bool enableMultiTouch;
    [SerializeField] 
    private ConnectedPlayers connectedPlayers;

    private bool need2Delete = true;

    private Participant _currentBuilder;


    [SerializeField]
    private TextMeshProUGUI brainstormTopic;

    private string[] topics =
    {
        "Global warming",
        "Plastic waste disposal",
        "Stop smoking"
    };
    private int currentTopic = -1;

    #endregion

    #region properties
    public List<Participant> Participants
    {
        get { return this.participants; }
        set { this.participants = value; }
    }

    public float FoundationTop { get; set; }
    #endregion

    #region methods
    private void Start()
    {
        currentDragObject = null;
        participants = connectedPlayers.GetConnectedPlayers();
       //Debug.Log("Connected players: " + participants.Count);

        //Mock participants for offline testing:
        //participants = new List<Participant>();
        //participants.Add(new Participant(System.Guid.NewGuid(), "Player 1", Color.red));
        //participants.Add(new Participant(System.Guid.NewGuid(), "Player 2", Color.green));
        //participants.Add(new Participant(System.Guid.NewGuid(), "Player 3", Color.blue));
        //participants.Add(new Participant(System.Guid.NewGuid(), "Player 4", Color.yellow));
        //participants.Add(new Participant(System.Guid.NewGuid(), "Player 5", Color.magenta));

        FoundationTop = this.foundationTop.position.y + (this.foundationTop.GetComponent<SpriteRenderer>().size.y / 2);

        currentBuilderIndex = 0;
        Input.multiTouchEnabled = enableMultiTouch;
        //NotifyNextBuilder(currentBuilderIndex);

        //timer.ResetTimer();

        brainstormTopic.text = GetRandomTopic();
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Insert))
        //{
        //    timer.ResetTimer();
        //}
        //TODO: Delete this



        //Call building time!
        if (Input.GetKeyDown(KeyCode.B))
        {
            newBuilderCalled.Invoke("Start building!");
        }

        //Restart
        if (Input.GetKeyDown(KeyCode.R))
        {
            var deleto = GameObject.FindGameObjectsWithTag("Block");
            foreach (var go in deleto)
            {
                Destroy(go);
            }
            deleto = GameObject.FindGameObjectsWithTag("Scaffolding");
            foreach (var go in deleto)
            {
                Destroy(go);
            }
            brainstormTopic.text = GetRandomTopic();
        }

        //Exit
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            EndGame();
        }
    }

    /// <summary>
    /// Gets a new random topic that isn't the current topic
    /// </summary>
    /// <returns>The string of the new topic</returns>
    private string GetRandomTopic()
    {
        int result = currentTopic;
        while (result == currentTopic)
        {
            result = Random.Range(0, topics.Length);
        }
        currentTopic = result;
        return topics[result];
    }

    /// <summary>
    /// Registers a new participant in the list of participants, get's called from the StringNetworkMessageCallbackDatabase
    /// </summary>
    /// <param name="participant">the new participant</param>
    public void RegisterNewParticipant(Participant participant)
    {
       //Debug.Log("Adding participant: " + participant.Name);
        if (participants == null)
            participants = new List<Participant>();
        participants.Add(participant);
    }

    public Participant GetParticipant(Guid participantId)
    {
        return participants.Find(p => p.Id == participantId);
    }

    /// <summary>
    /// Decide who, if any, the next builder is.
    /// If all builders have had a turn, activate the voting phase.
    /// </summary>
    public void DetermineNextBuilder()
    {
       //Debug.Log("Determine next builder: " + currentBuilderIndex + "  " + participants.Count);
        //Call next builder
        if (currentBuilderIndex < participants.Count)
        {
            //[PSEUDOCODE]
            NotifyNextBuilder(currentBuilderIndex);
            ++currentBuilderIndex;
        }
        //All participants have had a turn, commence voting phase
        //else
        //{
        //    EndGame();
        //    //StartVotingPhase();
        //}
    }

    /// <summary>
    /// Sends a notification to the next builder both on the screen and to the next builder's device
    /// </summary>
    /// <param name="builderParticipantIndex"></param>
    private void NotifyNextBuilder(int builderParticipantIndex)
    {
        Participant newBuilder = participants[builderParticipantIndex];
       //Debug.Log("New builder = " + newBuilder.Name);
        //newBuilderCalled.Invoke(newBuilder.Name);

        if (_currentBuilder != null)
        {
            UnityServerNetworkManager.Instance.SendMessageToPlayer(_currentBuilder.Id, new EventOnlyNetworkMessage(NetworkEvent.SERVER_STOP_BUILDER));
        }
       //Debug.Log("NewBuilder command send to " + newBuilder.Name + "   " + newBuilder.Id);
        UnityServerNetworkManager.Instance.SendMessageToPlayer(newBuilder.Id, new EventOnlyNetworkMessage(NetworkEvent.SERVER_ASSIGN_NEW_BUILDER));

        _currentBuilder = newBuilder;
    }

    /// <summary>
    /// Reactivates scene so next builder can go build.
    /// Also resets the timer.
    /// </summary>
    public void StartNewBuilderTime()
    {
        //timer.ResetTimer();
        //TODO: other activation stuff probably (enable rigidbodies etc.)
    }

    /// <summary>
    /// Tells game that a draggable can be picked up.
    /// </summary>
    public void AllowPickingUpBlocks()
    {
        currentDragObject = null;
    }

    public GameObject GetCurrentDragObject()
    {
        return currentDragObject;
    }

    /// <summary>
    /// Tells game that no new draggable can be picked up.
    /// </summary>
    /// <param name="currentDragObject"></param>
    public void RestrictPickingUpBlocks(GameObject currentDragObject)
    {
        this.currentDragObject = currentDragObject;
    }



    /// <summary>
    /// Activates the voting phase
    /// </summary>
    private void StartVotingPhase()
    {
    }

    private void EndGame()
    {
        Debug.Log("Shutdown");
        UnityServerNetworkManager.Instance.BroadcastMessage(new EventOnlyNetworkMessage(NetworkEvent.SERVER_END_GAME));
        Application.Quit();
    }

    #endregion
}

[System.Serializable]
public class StringBasedUnityEvent : UnityEvent<string> { }
