using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextInput : MonoBehaviour
{
    #region fields
    [SerializeField]
    private InputField textField;
    [SerializeField]
    private Dropdown playerSelection;
    [SerializeField]
    private BlockGenerator generator;

    private Participant[] mockParticipants = new Participant[5];
    #endregion

    #region methods
    // Start is called before the first frame update
    void Start()
    {
        //mockParticipants[0] = new Participant(1, "Player 1", Color.red);
        //mockParticipants[1] = new Participant(2, "Player 2", Color.green);
        //mockParticipants[2] = new Participant(3, "Player 3", Color.blue);
        //mockParticipants[3] = new Participant(4, "Player 4", Color.yellow);
        //mockParticipants[4] = new Participant(5, "Player 5", Color.magenta);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if(textField.text != "")
            {
                SendIdea(playerSelection.value + 1, textField.text);
            }
        }
    }

    public void SendIdea(int playerId, string ideaTitle)
    {
        generator.SpawnBlock(mockParticipants[playerId-1], ideaTitle);
    }
    #endregion
}
