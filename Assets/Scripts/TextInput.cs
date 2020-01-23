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
        mockParticipants[0] = new Participant(System.Guid.NewGuid(), "Player 1", new Color(0.8f, 0.2f, 0.2f));
        mockParticipants[1] = new Participant(System.Guid.NewGuid(), "Player 2", new Color(0.2f, 0.8f, 0.2f));
        mockParticipants[2] = new Participant(System.Guid.NewGuid(), "Player 3", new Color(0.2f, 0.2f, 0.8f));
        mockParticipants[3] = new Participant(System.Guid.NewGuid(), "Player 4", new Color(0.8f, 0.8f, 0.3f));
        mockParticipants[4] = new Participant(System.Guid.NewGuid(), "Player 5", Color.magenta);
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
