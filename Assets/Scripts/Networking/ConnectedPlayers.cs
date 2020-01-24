using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new connected players", menuName = "connected players")]
public class ConnectedPlayers : ScriptableObject
{
    private List<Participant> participants;

    public void AddConnectedPlayer(Participant participant)
    {
        if(participants == null)
            participants = new List<Participant>();

        participants.Add(participant);
    }

    public List<Participant> GetConnectedPlayers()
    {
        return participants;
    }
}
