using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField]
    private LobbyManager lobby;
    [SerializeField]
    private TutorialInfoButton popTutorialButton;
    [SerializeField]
    private TutorialInfoButton removeBlockScaffoldingTutorialButton;
    [SerializeField]
    private TutorialInfoButton tutorialStackButton;

    public List<Block> objectFloating;
    public List<Block> objectsOnGround;

    GameObject draggedBlockObject;
    Block draggedBlock;
    float draggedBlockTimer;

    //the block to be highlighted for the Remove Block Scaffolding Tutorial
    Block bottomBlock;
    GameObject bottomBlockObj;
    int blocksDropped = 0;

    bool stackntnsienabled;

    Color highlightColor; 

    void Start()
    {
        objectFloating = new List<Block>();
        objectsOnGround = new List<Block>();
        stackntnsienabled = false;
    }

    public void SetPopVisible()
    {
        popTutorialButton.gameObject.SetActive(true);
    }

    void Update()
    {
            RemovePopped();
        if (Input.GetMouseButton(0))
        {
            //ToggleHaloPopped(false); //used to turn off halo for all popped bubbles 
            
            draggedBlockTimer = 0;
            GetDraggedBlock();
            //Debug.Log(blocksDropped + " dropped");
            Debug.Log(objectsOnGround.Count);
        }

        if(blocksDropped == 2)
        {
            tutorialStackButton.transform.position = new Vector3(-8, 8, 0);
            stackntnsienabled = true;
        }

        draggedBlockTimer += 0.1f;
        CheckDraggedBlockMovement();
    }

    void GetDraggedBlock()
    {
        if (draggedBlockObject != null)
            return;
        foreach (Participant p in lobby.participantsConnected)
        {
            GameObject blockObject = lobby.participantBlocks[p.Id];
            Block block = blockObject.GetComponent<Block>();
            if (block.IsDragged())
            {
                if(block == bottomBlock){
                    DisableBottom();
                    removeBlockScaffoldingTutorialButton.HighlightControl(false);
                    removeBlockScaffoldingTutorialButton.gameObject.SetActive(false);
                }
                Debug.Log("Here it iisss");
                draggedBlock = block;
                draggedBlockObject = blockObject;
            }
        }
    }

    void CheckDraggedBlockMovement()
    {
        if (draggedBlockObject == null || draggedBlockTimer < 2)
            return;
        if (draggedBlock.IsDragged())
        {
            return;
        }
        Rigidbody2D rb = draggedBlockObject.GetComponent<Rigidbody2D>();
        if(rb.velocity.magnitude < 0.1)
        {
            Debug.Log("White people!");
            BeginBottomBlockScaffTutorial();
            draggedBlockObject = null;
            draggedBlock = null;
        }
    }

    void RemovePopped()
    {
        foreach(Block b in objectFloating.ToList())
        {
            if (b.CheckIfBubblePopped())
            {
                b.GetComponent<ColorChanger>().Toggle(false);
                objectFloating.Remove(b);
                blocksDropped++;
                objectsOnGround.Add(b);
                ToggleHaloBehaviour(tutorialStackButton.isOpened,b.gameObject);
            }
        }
    }

    public void ToggleHaloPopped(bool mode)
    {
        foreach (GameObject b in CheckForStillFloating(true))
        {
            ToggleHaloBehaviour(mode, b);
        }
    }
    public void HighlightPopTutorialBlocks()
    {
        Debug.Log("I WAS IN HERE WHYYYYYYYYYYYY XXXXXXXXXXXXXXXXXXXXXXXXXXXXX");

        DisableBottom();
        removeBlockScaffoldingTutorialButton.HighlightControl(false);
        removeBlockScaffoldingTutorialButton.gameObject.SetActive(false);
        DisableOnGround();
        tutorialStackButton.HighlightControl(false);
        Debug.Log("Takam");
        for (int i = 0; i < objectFloating.Count; i++)
        {
            ToggleHaloBehaviour (popTutorialButton.isOpened, objectFloating[i].gameObject);
        }
        //foreach(GameObject b in objectFloating)
        //{
        //    ToggleHaloBehaviour(popTutorialButton.isOpened, b);
        //}
    }

    public void BeginBlockStackingTutorial()
    {
        Debug.Log("I WAS IN HERE WHYYYYYYYYYYYY !!!!!!!!!!!!!!!!!!");
        DisableBottom();
        removeBlockScaffoldingTutorialButton.HighlightControl(false);
       removeBlockScaffoldingTutorialButton.gameObject.SetActive(false);
        DisableFloatingBlocks();
        popTutorialButton.HighlightControl(false);
        for (int i = 0; i < objectsOnGround.Count; i++)
        {
            ToggleHaloBehaviour(tutorialStackButton.isOpened, objectsOnGround[i].gameObject);
        }
        //foreach(GameObject b in objectFloating)
        //{
        //    ToggleHaloBehaviour(popTutorialButton.isOpened, b);
        //}

        if (!stackntnsienabled)
        {
       
        }
    }
    public void BottomScaffToggleTutorial()
    {
        Debug.Log("Fuck me " + bottomBlock);
        DisableOnGround();
        tutorialStackButton.HighlightControl(false);
        DisableFloatingBlocks();
        popTutorialButton.HighlightControl(false);
        Debug.Log(removeBlockScaffoldingTutorialButton.isOpened + " is iiiiiiiiiiiiiiiiiiit");
        if (bottomBlockObj != null)
        {
            ToggleHaloBehaviour(removeBlockScaffoldingTutorialButton.isOpened, bottomBlockObj);
        }
        //bottomBlockObj.GetComponent<ColorChanger>().Toggle(removeBlockScaffoldingTutorialButton.isOpened);
    }
    void DisableFloatingBlocks()
    {
        foreach(Block b in objectFloating)
        {
            ToggleHaloBehaviour(false, b.gameObject);
        }
    }

    void DisableBottom()
    {
        if(bottomBlockObj != null) {
            bottomBlockObj.GetComponent<ColorChanger>().Toggle(false);
        }
        bottomBlock = null;
        bottomBlockObj = null;
        Debug.Log("What the guck");
    }

    void DisableOnGround()
    {
        foreach(Block b in objectsOnGround)
        {
            ToggleHaloBehaviour(false, b.gameObject);
        }
    }

    private void ToggleHaloBehaviour(bool mode, GameObject block)
    {
        //Behaviour beh = (Behaviour)block.GetComponent("Halo");
        Debug.Log(block);
        Debug.Log(block.GetComponent<ColorChanger>());
        ColorChanger c = block.GetComponent<ColorChanger>();
        
        c.Toggle(mode);
        Debug.Log("Toggled" + mode);
        //beh.enabled = mode;
    }



    public List<GameObject> CheckForStillFloating(bool mode)
    {
        List<GameObject> floatingBlocks = new List<GameObject>();

        foreach(Participant p in lobby.participantsConnected)
        {
            GameObject block = lobby.participantBlocks[p.Id];
            if (mode == block.GetComponent<Block>().CheckIfBubblePopped())
            {
                floatingBlocks.Add(block);
            }
        }
        return floatingBlocks;
    }

    public void BeginBottomBlockScaffTutorial()
    {
        if (draggedBlockObject == null)
            return;
        if (!draggedBlock.IsOtherBlockOnBottom())
            return;
        foreach (Participant p in lobby.participantsConnected)
        {
            GameObject block = lobby.participantBlocks[p.Id];
           // Debug.Log("IN WEST PHILADELPHIA BORN AND RASED " + draggedBlock.BottomBlock());
            if (draggedBlock.BottomBlock() == block)
            {
                Debug.Log("what the fuck happened here");
                bottomBlockObj = block;
                bottomBlock = block.GetComponent<Block>();
                //  Debug.Log("AWOOOOOOOOOOOO 5 6 7 O 9");
                //  //Behaviour beh = (Behaviour)block.GetComponent("Halo");
                ////  beh.enabled = true;
                // ColorChanger c = block.GetComponent<ColorChanger>();
                // c.Toggle(true);
                removeBlockScaffoldingTutorialButton.gameObject.SetActive(true);
                float posy = block.transform.position.y;
                if (posy < 3)
                {
                    posy = 3;
                }
                removeBlockScaffoldingTutorialButton.transform.position = new Vector3(bottomBlock.GetWidth() * 2 + block.transform.position.x, posy);
            }
            //Debug.Log("Sokka");
        }
    }

   

   

    public void OnNewHighlight()
    {
        if(objectFloating.Count != 0)
        {
            for(int i = 0; i < objectFloating.Count; i++)
            {

            }
        }
    }

    void DisablePopTutorialBtn()
    {
        popTutorialButton.GetComponent<TutorialInfoButton>().HighlightControl(false);
    }
}