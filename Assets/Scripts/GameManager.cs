using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoSingleton<GameManager>
{
    #region fields
    [SerializeField]
    public float buildingTime;
    [SerializeField]
    public UnityEvent buildingTimeCompleted;
    [SerializeField]
    public UnityEvent buildingPhaseCompleted;

    private List<Participant> participants; 
    private int currentBuilder;
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

        currentBuilder = 0;
        //[PSEUDOCODE]
        //Send buildermessage participants[currentBuilder]

        //timer = buildingTime; 
    }

    private void Update()
    {
        
    }
    #endregion
}
