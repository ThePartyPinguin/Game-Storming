using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Block : Draggable
{
    #region fields
    [SerializeField]
    private string idea;
    [SerializeField]
    private Tower tower;
    private Participant owner;
    [SerializeField]
    private GameObject scaffoldPrefab;
    [SerializeField]
    private TrailRenderer blockTrail;
    [SerializeField]
    private ParticleSystem teleportParticles;

    [SerializeField]
    private TextMeshPro textVisual;
    [SerializeField]
    private GameObject blockBubble;
    [SerializeField]
    private BoxCollider2D blockCollider;
    [SerializeField]
    private TextMeshPro importanceDisplay;
    private float BlockReleaseHeight = 0.1f;
    [SerializeField]
    private float AutodropTimerValue = 2.0f;
    [SerializeField]
    private float BottomCheckSize = 0.2f;

    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private HingeJoint2D towerJoint;
    private GameObject bottomBlock;

    private int importance;
    private bool isConnected;
    private float respawnHeight;
    private bool isWaitingForSafeSpot;
    private Coroutine currentCoroutine;
    private float AutodropCheckDelayTimer;
    [SerializeField]
    private bool InvalidSpotWarning;
    [SerializeField]
    private float WarningFlashSpeed = 0.5f;
    #endregion

    #region properties
    public string Idea
    {
        get { return this.idea; }
        set
        {
            this.idea = value;
            this.textVisual.text = this.textVisual ? idea : "error";
        }
    }

    public Tower Tower { get { return this.tower; } }

    public Participant Owner
    {
        get { return this.owner; }
        set { this.owner = value; }
    }
    #endregion

    #region methods
    private new void Start()
    {
        //Cache components
        spriteRenderer = GetComponent<SpriteRenderer>();
        towerJoint = GetComponent<HingeJoint2D>();
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true;
        tower = null;
        isConnected = false;
        respawnHeight = Camera.main.orthographicSize + Camera.main.transform.position.y + (GetHeight()) + 1;

        importance = 1;
        importanceDisplay.transform.parent.localPosition = new Vector2(spriteRenderer.size.x / 2 - 0.175f, spriteRenderer.size.y /2 - 0.175f);

        InvokeRepeating("CheckOutOfWorld", 1, 1);

        base.Start();
    }

    private new void Update()
    {
        if (blockCollider.enabled)
        {
            Debug.DrawLine(transform.position, new Vector2(transform.position.x + 1, transform.position.y + 1), Color.green, 0.25f);
        } else
        {
        Debug.DrawLine(transform.position, new Vector2(transform.position.x + 1, transform.position.y + 1), Color.red, 0.25f);

        }

        if (AutodropCheckDelayTimer > 0f)
        {
            AutodropCheckDelayTimer -= Time.deltaTime;
        }
        else
        {
            if (this.isDragged)
            {
                if (CheckTowerUnderneath())
                {
                    this.transform.position = new Vector2(this.transform.position.x, this.transform.position.y + BlockReleaseHeight);
                    this.isDragged = false;
                }
                /*if (CheckTowerSides())
                {
                    this.isDragged = false;
                }*/
            }
            else
            {
                if (CheckTowerUnderneath() /*|| CheckTowerSides()*/)
                {
                    AutodropCheckDelayTimer = AutodropTimerValue;
                }
            }
        }

        if (InvalidSpotWarning)
        {
            this.spriteRenderer.color = Color.Lerp(Color.yellow, Color.red, Mathf.PingPong(Time.time, WarningFlashSpeed));
        }
        else
        {
            this.spriteRenderer.color = this.owner.Color;
        }


        base.Update();
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        //ensure the bottomBlock variable is cleared
        if(collision.transform.position.y < this.transform.position.y && collision.gameObject != bottomBlock)
        {
            bottomBlock = null;
        }

        //Shouldn't do anything if the block is connected to a tower
        if (isConnected) { return; }

        //If the block hits the foundation, the block becomes a solid static foundation block
        if (collision.gameObject.CompareTag("Foundation") && tower == null)
        {
            if (currentCoroutine != null) { StopCoroutine(currentCoroutine); }
            Tower newTower = new Tower(this);
            this.tower = newTower;

            //this.isConnected = true;

            ////Make block fall onto a side and then become static
            //towerJoint.enabled = true;

            //Vector2 contactPoint = transform.InverseTransformPoint(collision.GetContact(0).point);
            //Vector2 normalisedAnchor = new Vector2(
            //    contactPoint.x < 1 ? Mathf.Max(contactPoint.x, -0.5f) : Mathf.Min(contactPoint.x, 0.5f),
            //    contactPoint.y < 1 ? Mathf.Max(contactPoint.y, -0.5f) : Mathf.Min(contactPoint.y, 0.5f));
            //Vector2 sizeMultiplier = spriteRenderer.size;

            //towerJoint.anchor = Vector2.Scale(normalisedAnchor, sizeMultiplier);
            //towerJoint.connectedBody = collision.rigidbody;
            //StartCoroutine(SnapToFoundation());

            //Release mouse

            base.OnMouseUp();
        }
        //If the block hits a tower, it becomes part of that tower
        else if (collision.gameObject.CompareTag("Block") && collision.gameObject.GetComponent<Block>()) //Extra check for if it's done
        {
            Block otherBlock = collision.gameObject.GetComponent<Block>();
            currentCoroutine = StartCoroutine(SnapToTower(otherBlock.Tower));

            //Debug.Log("ON THE PLAYGROUND IS WHERE I SPEND MOST OF MY DAYS");

            //adding half the height to prevent bottomBlock being assigned a value when the collision block is in weird position
            if(otherBlock.transform.position.y + otherBlock.GetHeight()/2 < this.transform.position.y)
            {
                //Debug.Log("CHILLIN, RELAXING ALL COOL");
                bottomBlock = collision.gameObject;
            }
        }
    }

    /// <summary>
    /// Returns the current height of the block, taking into account the rotation.
    /// </summary>
    public float GetHeight()
    {
        float functionalRotation = (transform.eulerAngles.z % 180);
        bool isRotated = 45 <= functionalRotation && functionalRotation < 135;
        return (isRotated ? spriteRenderer.size.x : spriteRenderer.size.y);
    }

    /// <summary>
    /// Returns the current width of the block, taking into account the rotation.
    /// </summary>
    public float GetWidth()
    {
        float functionalRotation = (transform.eulerAngles.z % 180);
        bool isRotated = 45 <= functionalRotation && functionalRotation < 135;
        return (isRotated ? spriteRenderer.size.y : spriteRenderer.size.x);
    }

    /// <summary>
    /// Increases the importance value of the block and visually updates it.
    /// </summary>
    public void IncreaseImportance()
    {
        //Activate the importancedisplay on first time
        if (importance == 1) { importanceDisplay.transform.parent.gameObject.SetActive(true); }

        ++importance;
        importanceDisplay.text = importance.ToString();
    }

    /// <summary>
    /// Detach a block from the tower that it's in, so that it can get connected to another tower.
    /// </summary>
    public void DetachFromTower()
    {
        if (IsOtherBlockOnTop()) { Scaffold(); }

        tower = null;
        //this.isConnected = false;
        //StartCoroutine(ReleaseFromFoundation());

        if (this.GetComponentInChildren<BlockBubble>() != null)
        {
            this.transform.position = new Vector3(this.blockBubble.transform.position.x, this.blockBubble.transform.position.y, 0.1f);   
        }
    }

    /// <summary>
    /// Places a scaffolding piece and initialises it to match the size of the original block it is replacing
    /// </summary>
    public void Scaffold()
    {
        var scaffold = Instantiate(scaffoldPrefab, transform.position, Quaternion.identity);
        scaffold.GetComponent<SpriteRenderer>().size *= new Vector2(GetWidth() - 0.05f, GetHeight() - 0.05f);
    }

    /// <summary>
    /// checks in a small area underneath the block to see if its hovering above a tower. 
    /// </summary>
    /// <returns></returns>
    public bool CheckTowerUnderneath()
    {
        RaycastHit2D h = Physics2D.Raycast(new Vector2(spriteRenderer.bounds.center.x - (spriteRenderer.bounds.extents.x / 5), (spriteRenderer.bounds.center.y - spriteRenderer.bounds.extents.y - 0.2f)), Vector2.right / 5, BottomCheckSize);

        if (h.collider != null && h.collider.tag == "Block" && h.transform.gameObject.layer != 9)
        {
            return true;
        }
        else return false;
    }

    /// <summary>
    /// checks on the sides of the block if there are any blocks. 
    /// </summary>
    /// <returns></returns>
    public bool CheckTowerSides()
    {
        RaycastHit2D lefthit = Physics2D.Raycast(new Vector2(spriteRenderer.bounds.min.x - 0.1f, spriteRenderer.bounds.min.y - 0.1f), Vector2.up, spriteRenderer.bounds.size.y);
        RaycastHit2D righthit = Physics2D.Raycast(new Vector2(spriteRenderer.bounds.max.x + 0.1f, spriteRenderer.bounds.max.y + 0.1f), Vector2.down, spriteRenderer.bounds.size.y);

        if(lefthit.collider != null && lefthit.collider.tag == "Block" && lefthit.transform.gameObject.layer != 9)
        {
            return true;
        }
        if(righthit.collider != null && righthit.collider.tag == "Block" && righthit.transform.gameObject.layer != 9)
        {
            return true;
        }
        return false;
    }


    /// <summary>
    /// Checks whether or not another block lies on top of this block
    /// </summary>
    /// <returns>True if another block lies on top of this block, false if not</returns>
    public bool IsOtherBlockOnTop()
    {
        if (rb.velocity.magnitude > 0.1f || blockBubble) { return false; }

        var checkHeight = 0.75f;
        var checkWidthCutOff = 0.1f;

        var checkBoxCenter = new Vector2(transform.position.x, transform.position.y + (GetHeight() / 2) + checkHeight);
        var checkBoxHalfExtents = new Vector3(GetWidth()/2, GetHeight()/10);
        var checkBoxCornerA = new Vector2(checkBoxCenter.x - checkBoxHalfExtents.x + checkWidthCutOff, checkBoxCenter.y - checkBoxHalfExtents.y);
        var checkBoxCornerB = new Vector2(checkBoxCenter.x + checkBoxHalfExtents.x - checkWidthCutOff, checkBoxCenter.y + checkBoxHalfExtents.y);
        Debug.DrawLine(checkBoxCornerA, checkBoxCornerB, Color.blue, 2, false);
        return (Physics2D.OverlapArea(checkBoxCornerA, checkBoxCornerB) != null);
    }

    public bool IsOtherBlockOnBottom()
    {
        if (rigidBody.velocity.magnitude > 0.1f || blockBubble) { return false; }

        var checkHeight = 0.75f;
        var checkWidthCutOff = 0.1f;

        var checkBoxCenter = new Vector2(transform.position.x, transform.position.y - (GetHeight() / 2) + checkHeight);
        var checkBoxHalfExtents = new Vector3(GetWidth() / 2, GetHeight() / 10);
        var checkBoxCornerA = new Vector2(checkBoxCenter.x - checkBoxHalfExtents.x + checkWidthCutOff, checkBoxCenter.y - checkBoxHalfExtents.y);
        var checkBoxCornerB = new Vector2(checkBoxCenter.x + checkBoxHalfExtents.x - checkWidthCutOff, checkBoxCenter.y + checkBoxHalfExtents.y);
        Debug.DrawLine(checkBoxCornerA, checkBoxCornerB, Color.blue, 2, false);
        return (Physics2D.OverlapArea(checkBoxCornerA, checkBoxCornerB) != null);
    }

    /// <summary>
    /// Destroy the bubble and activate the block physics so it can be dragged around.
    /// </summary>
    public void DetachAndDestroyBubble()
    {
        if (this.blockBubble != null)
        {
            //Pop the bubble
            Destroy(blockBubble);

            //Activate block physics
            rb.isKinematic = false;
            blockCollider.enabled = true;

            //Render block in front of bubbleBlocks
            spriteRenderer.sortingOrder = 4;
            GetComponentInChildren<TextMeshPro>().sortingOrder = 5;
            GetComponentInChildren<TrailRenderer>().emitting = true;
            this.transform.parent = null;
        }
    }

    public override void OnMouseDown()
    {
        this.DetachAndDestroyBubble();
        //Block[] b = GameObject.FindObjectsOfType<Block>();
        //for (int i = 0; i < b.Length; i++)
        //{
        //    b[i].isDragged = false;
        //    gameObject.layer = originalLayer;
        //    dragJoint.enabled = false;
        //}
        isDragged = true;
        base.OnMouseDown();
    }

    protected override void OnMouseUp()
    {
        if (this.isConnected) 
        {
            return;
        }
        CheckIfBlockIsInSafeSpot();
    }

    private void CheckIfBlockIsInSafeSpot()
    {
        var checkBoxHalfExtents = new Vector3(GetWidth() / 2, GetHeight() / 2);
        var checkBoxCornerA = new Vector2(transform.position.x - checkBoxHalfExtents.x, transform.position.y - checkBoxHalfExtents.y + 0.1f);
        var checkBoxCornerB = new Vector2(transform.position.x + checkBoxHalfExtents.x, transform.position.y + checkBoxHalfExtents.y + 0.1f);
        Debug.DrawLine(checkBoxCornerA, checkBoxCornerB, Color.green, 2, false);
        var layerMask = ~(1 << 9);
        var hit = Physics2D.OverlapArea(checkBoxCornerA, checkBoxCornerB, layerMask);
        if (hit != null && hit.GetComponentInChildren<BlockBubble>() == null && !hit.gameObject.Equals(this.gameObject))
        {
            if (!isWaitingForSafeSpot)
            {
                StartCoroutine(ActivateCollisionWhenSafe());
            }
        }
        else
        {
            isDragged = false;
            isWaitingForSafeSpot = false;
            base.OnMouseUp();
        }
    }

    public bool CheckIfBubblePopped()
    {
        if(blockBubble == null)
            return true;
        return false;
    }

    public int CalculateScore()
    {
        throw new System.NotImplementedException();
    }

    public int DragId()
    {
        throw new System.NotImplementedException();
    }

    public bool IsDragged()
    {
        return isDragged;
    }

    public GameObject BottomBlock()
    {
        return bottomBlock;
    }

    /// <summary>
    /// Activates a recurring check to see if the block is safe to be put at its location.
    /// </summary>
    /// <returns></returns>
    private IEnumerator ActivateCollisionWhenSafe()
    {
        rb.isKinematic = false; 
        blockCollider.enabled = false;
        isWaitingForSafeSpot = true;
        InvalidSpotWarning = true;
        InvokeRepeating("CheckIfBlockIsInSafeSpot", 0.5f, 0.25f);
        yield return new WaitUntil(() => (isWaitingForSafeSpot == false));
        blockCollider.enabled = true;
        InvalidSpotWarning = false;
        CancelInvoke("CheckIfBlockIsInSafeSpot");
    }


    /// <summary>
    /// Stops all horizontal movement a block has so it can't be thrown into other towers
    /// </summary>
    public void StopHorizontalMovement()
    {
        rb.velocity = new Vector2(rb.velocity.x / 100, rb.velocity.y / 10);
    }

    /// <summary>
    /// Waits until the block lies still and then assigns it to the tower.
    /// </summary>
    /// <param name="otherTower">The tower the block will snap to</param>
    private IEnumerator SnapToTower(Tower otherTower)
    {
        yield return new WaitUntil(() => (Mathf.Approximately(rb.velocity.magnitude, 0f) && Mathf.Approximately(rb.angularVelocity, 0f)));
        //this.tower = otherTower;
        //otherTower.AddBlock(this);
        currentCoroutine = null;
        base.OnMouseUp();
    }

    ///// <summary>
    ///// Snap block to ground so that it's a solid foundation for a tower.
    ///// Makes the block lay flat on the ground and be not affected by physics.
    ///// </summary>
    //private IEnumerator SnapToFoundation()
    //{
    //    //Enable joint to make the block not move anymore, only rotate
    //    GetComponent<TargetJoint2D>().enabled = false;
    //    rigidBody.velocity = Vector2.zero;
    //    rigidBody.angularVelocity = 0.1f;

    //    //Wait until the block lays flat on one of its sides
    //    yield return new WaitUntil(() => (Mathf.Abs(transform.eulerAngles.z % 90) < 2f || Mathf.Abs(transform.eulerAngles.z % 90) > 88));

    //    //Definitively rotate and position the block to negate small rotation and position errors
    //    rigidBody.isKinematic = true;
    //    rigidBody.velocity = Vector2.zero;
    //    rigidBody.angularVelocity = 0f;
    //    transform.eulerAngles = new Vector3(0, 0, Mathf.Round(transform.eulerAngles.z / 90) * 90);
    //    Debug.Log(GetHeight());
    //    transform.position = new Vector3(transform.position.x, GameManager.Instance.FoundationTop + GetHeight() / 2, 1);

    //    //Disable the rotatejoint
    //    towerJoint.enabled = false;
    //}

    ///// <summary>
    ///// Removes a block from foundation if there are no other blocks in the tower.
    ///// </summary>
    //private IEnumerator ReleaseFromFoundation()
    //{
    //    rigidBody.isKinematic = false;
    //    blockCollider.enabled = false;

    //    var minPos = transform.position.y - 0.5f ;


    //    yield return new WaitUntil(() => (transform.position.y >= (GameManager.Instance.FoundationTop + 0.05f + GetHeight()/2) || transform.position.y <= minPos || transform.position.y < -10));
    //    this.isConnected = false;
    //    blockCollider.enabled = true;
    //}

    /// <summary>
    /// Checks if the block is falling out of world and resets it if necessary
    /// </summary>
    private void CheckOutOfWorld()
    {
        if (transform.position.y < -10)
        {
            StartCoroutine(TeleportBlock());
        }
    }

    private void OnDrawGizmos()
    {
        if (this.isDragged)
        {
            Gizmos.DrawRay(new Vector2(spriteRenderer.bounds.min.x - 0.1f, spriteRenderer.bounds.min.y - 0.1f), Vector3.up);
            Gizmos.DrawRay(new Vector2(spriteRenderer.bounds.center.x - (spriteRenderer.bounds.extents.x / 5), (spriteRenderer.bounds.center.y - spriteRenderer.bounds.extents.y - 0.2f)), Vector2.right / 5);
        }
    }

    private IEnumerator TeleportBlock()
    {
        blockTrail.enabled = false;
        blockTrail.Clear();
        teleportParticles.Play();
        transform.rotation = Quaternion.identity;
        transform.position = new Vector2(transform.position.x, respawnHeight);
        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(0.4f);
        blockCollider.enabled = true;
        teleportParticles.Stop(false, ParticleSystemStopBehavior.StopEmitting);
        blockTrail.Clear();
        blockTrail.enabled = true;
        isDragged = false;
    }
    #endregion
}