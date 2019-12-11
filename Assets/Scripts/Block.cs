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
    private TextMeshPro textVisual;
    [SerializeField]
    private GameObject blockBubble;
    [SerializeField]
    private BoxCollider2D blockCollider;

    private Rigidbody2D rigidBody;
    private HingeJoint2D towerJoint;

    private bool isConnected;
    private Coroutine currentCoroutine;
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
    private void Start()
    {
        //Cache components
        towerJoint = GetComponent<HingeJoint2D>();
        rigidBody = GetComponent<Rigidbody2D>();
        rigidBody.isKinematic = true;
        tower = null;
        isConnected = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Shouldn't do anything if the block is connected to a tower
        if (isConnected) { return; }

        //If the block hits the foundation, the block becomes a solid static foundation block
        if (collision.gameObject.CompareTag("Foundation") && tower == null)
        {
            if (currentCoroutine != null) { StopCoroutine(currentCoroutine); }
            Tower newTower = new Tower(this);
            this.tower = newTower;
            this.isConnected = true;
            
            //Make block fall onto a side and then become static
            towerJoint.enabled = true;
            
            Vector2 contactPoint = transform.InverseTransformPoint(collision.GetContact(0).point);
            Vector2 normalisedAnchor = new Vector2(
                contactPoint.x < 1 ? Mathf.Max(contactPoint.x, -0.5f) : Mathf.Min(contactPoint.x, 0.5f),
                contactPoint.y < 1 ? Mathf.Max(contactPoint.y, -0.5f) : Mathf.Min(contactPoint.y, 0.5f));
            Vector2 sizeMultiplier = GetComponent<SpriteRenderer>().size;

            towerJoint.anchor = Vector2.Scale(normalisedAnchor, sizeMultiplier);
            towerJoint.connectedBody = collision.rigidbody;
            StartCoroutine(SnapToFoundation());

            //Release mouse
            base.OnMouseUp();
        }

        //If the block hits a tower, it becomes part of that tower
        else if (collision.gameObject.CompareTag("Block") && collision.gameObject.GetComponent<Block>()) //Extra check for if it's done
        {
            Block otherBlock = collision.gameObject.GetComponent<Block>();
            currentCoroutine = StartCoroutine(SnapToTower(otherBlock.Tower));
        }
    }

    /// <summary>
    /// Returns the current height of the block, taking into account the rotation.
    /// </summary>
    public float GetHeight()
    {
        float functionalRotation = (transform.eulerAngles.z % 180);
        bool isHorizontal = 45 <= functionalRotation && functionalRotation < 135;
        var renderer = GetComponent<SpriteRenderer>();
        return (isHorizontal ? renderer.size.x : renderer.size.y);
    }

    /// <summary>
    /// Detach a block from the tower that it's in, so that it can get connected to another tower.
    /// </summary>
    public void DetachFromTower()
    {
        Scaffold();
        tower = null;
        this.isConnected = false;
        StartCoroutine(ReleaseFromFoundation());
        if(this.GetComponentInChildren<BlockBubble>() != null)
        {
            this.transform.position = new Vector3(this.blockBubble.transform.position.x, this.blockBubble.transform.position.y, 0.1f);   
        }
    }

    public void Scaffold()
    {
        if (BlockAbove())
        {
            
        }
    }

    public bool BlockAbove()
    {
        return true;
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
            rigidBody.isKinematic = false;
            blockCollider.enabled = true;

            //Render block in front of bubbleBlocks
            GetComponent<SpriteRenderer>().sortingOrder = 4;
            GetComponentInChildren<TextMeshPro>().sortingOrder = 5;
            this.transform.parent = null;
        }
    }

    public override void OnMouseDown()
    {
        this.DetachAndDestroyBubble();
        base.OnMouseDown();
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
        throw new System.NotImplementedException();
    }

    /// <summary>
    /// Snap block to ground so that it's a solid foundation for a tower.
    /// Makes the block lay flat on the ground and be not affected by physics.
    /// </summary>
    private IEnumerator SnapToFoundation()
    {
        //Enable joint to make the block not move anymore, only rotate
        GetComponent<TargetJoint2D>().enabled = false;
        rigidBody.velocity = Vector2.zero;
        rigidBody.angularVelocity = 0.1f;
        
        //Wait until the block lays flat on one of its sides
        yield return new WaitUntil(() => (Mathf.Abs(transform.eulerAngles.z % 90) < 2f || Mathf.Abs(transform.eulerAngles.z % 90) > 88));

        //Definitively rotate and position the block to negate small rotation and position errors
        rigidBody.isKinematic = true;
        rigidBody.velocity = Vector2.zero;
        rigidBody.angularVelocity = 0f;
        transform.eulerAngles = new Vector3(0, 0, Mathf.Round(transform.eulerAngles.z / 90) * 90);
        Debug.Log(GetHeight());
        transform.position = new Vector3(transform.position.x, GameManager.Instance.FoundationTop + GetHeight() / 2, 1);

        //Disable the rotatejoint
        towerJoint.enabled = false;
    }

    /// <summary>
    /// Removes a block from foundation if there are no other blocks in the tower.
    /// </summary>
    private IEnumerator ReleaseFromFoundation()
    {
        rigidBody.isKinematic = false;
        GetComponent<Collider2D>().enabled = false;

        yield return new WaitUntil(() => (transform.position.y >= (GameManager.Instance.FoundationTop + 0.05f + GetHeight()/2)));
        GetComponent<Collider2D>().enabled = true;
    }

    /// <summary>
    /// Waits until the block lies still and then assigns it to the tower.
    /// </summary>
    /// <param name="otherTower">The tower the block will snap to</param>
    private IEnumerator SnapToTower(Tower otherTower)
    {
        yield return new WaitUntil(() => (Mathf.Approximately(rigidBody.velocity.magnitude, 0f) && Mathf.Approximately(rigidBody.angularVelocity, 0f)));
        this.tower = otherTower;
        otherTower.AddBlock(this);
        currentCoroutine = null;
        base.OnMouseUp();
    }
    #endregion
}