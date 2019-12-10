using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoSingleton<GameManager>
{
    #region fields
    private List<Participant> participants; 
    private int currentBuilderIndex;
    [SerializeField]
    private CountdownTimer timer;
    

    [SerializeField]
    private StringBasedUnityEvent newBuilderCalled;
    #endregion

    #region properties
    public List<Participant> Participants { get; set; }
    #endregion

    #region methods
    private void Start()
    {
        participants = new List<Participant>();
        participants.Add(new Participant(1, "Player 1", Color.red));
        participants.Add(new Participant(2, "Player 2", Color.green));
        participants.Add(new Participant(3, "Player 3", Color.blue));
        participants.Add(new Participant(4, "Player 4", Color.yellow));
        participants.Add(new Participant(5, "Player 5", Color.magenta));

        currentBuilderIndex = 0;
        NotifyNextBuilder(currentBuilderIndex);
    }

    /// <summary>
    /// Decide who, if any, the next builder is.
    /// If all builders have had a turn, activate the voting phase.
    /// </summary>
    public void DetermineNextBuilder()
    {
        //Call next builder
        if (currentBuilderIndex != (participants.Count - 1))
        {
            ++currentBuilderIndex;
            //[PSEUDOCODE]
            NotifyNextBuilder(currentBuilderIndex);
        }
        //All participants have had a turn, commence voting phase
        else
        {
            StartVotingPhase();
        }
    }

    /// <summary>
    /// Sends a notification to the next builder both on the screen and to the next builder's device
    /// </summary>
    /// <param name="builderParticipantIndex"></param>
    private void NotifyNextBuilder(int builderParticipantIndex)
    {
        var builderName = participants[builderParticipantIndex].Name;
        Debug.Log(builderName);
        newBuilderCalled.Invoke(builderName);
        //TODO: Networking notify next builder phone
    }

    /// <summary>
    /// Reactivates scene so next builder can go build.
    /// Also resets the timer.
    /// </summary>
    public void StartNewBuilderTime()
    {
        timer.ResetTimer();
        //TODO: other activation stuff probably (enable rigidbodies etc.)
    }

    /// <summary>
    /// Activates the voting phase
    /// </summary>
    private void StartVotingPhase()
    {
        throw new System.NotImplementedException();
    }

    #endregion
}

[System.Serializable]
public class StringBasedUnityEvent : UnityEvent<string> { }
