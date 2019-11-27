using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextInput : MonoBehaviour
{
    #region fields
    [SerializeField]
    private bool isFocused;
    [SerializeField]
    InputField textField;
    [SerializeField]
    Dropdown playerSelection;
    [SerializeField]
    GameObject blockPrefab;
    [SerializeField]
    Transform spawnLocation;

    Participant[] mockParticipants = new Participant[5];
    #endregion

    #region methods
    // Start is called before the first frame update
    void Start()
    {
        mockParticipants[0] = new Participant(1, "Player 1");
        mockParticipants[1] = new Participant(2, "Player 2");
        mockParticipants[2] = new Participant(3, "Player 3");
        mockParticipants[3] = new Participant(4, "Player 4");
        mockParticipants[4] = new Participant(5, "Player 5");
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
        GameObject block = Instantiate(blockPrefab, spawnLocation);

        block.GetComponent<Block>().setParticipant(mockParticipants[playerSelection.value]);
        block.GetComponent<Block>().setIdea(ideaTitle);
        mockParticipants[playerSelection.value].addBlock(block.GetComponent<Block>());
        Debug.Log("Block Created: [Owner: " + mockParticipants[playerSelection.value].ToString() + "] [BlockTitle: " + ideaTitle + "]");
    }
    #endregion
}
