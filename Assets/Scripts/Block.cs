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
    Participant owner;

    [SerializeField]
    private TextMeshPro textVisual;
    [SerializeField]
    private GameObject blockbubble;

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

    public Tower Tower { get; }

    public Participant Participant { get; set; }
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
            towerJoint.anchor = new Vector2(
                contactPoint.x < 1 ? Mathf.Max(contactPoint.x, -0.5f) : Mathf.Min(contactPoint.x, 0.5f),
                contactPoint.y < 1 ? Mathf.Max(contactPoint.y, -0.5f) : Mathf.Min(contactPoint.y, 0.5f));
            towerJoint.connectedBody = collision.rigidbody;
            StartCoroutine(SnapToFoundation());

            //Release mouse
            base.OnMouseUp();
        }

        //If the block hits a tower, it becomes part of that tower
        else if (collision.gameObject.CompareTag("Block") && collision.gameObject.GetComponent<Block>()) //Extra check for if it's done
        {
            //Debug.Log("HitBlock" + Time.time);
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
        return (isHorizontal ? transform.localScale.x : transform.localScale.y);
    }

    /// <summary>
    /// Detach a block from the tower that it's in, so that it can get connected to another tower.
    /// </summary>
    public void DetachFromTower()
    {
        tower = null;
        this.isConnected = false;
        StartCoroutine(ReleaseFromFoundation());
        if(this.GetComponentInChildren<BlockBubble>() != null)
        {
            this.transform.position = new Vector3(this.blockbubble.transform.position.x, this.blockbubble.transform.position.y, 0.1f);   
        }
    }

    /// <summary>
    /// Destroy the bubble and activate the block physics so it can be dragged around.
    /// </summary>
    public void DetachAndDestroyBubble()
    {
        if (this.blockbubble != null)
        {
            Destroy(blockbubble);
            rigidBody.isKinematic = false;
        }
    }

    public override void OnMouseDown()
    {
        Debug.Log("clicked block");
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
        GetComponent<TargetJoint2D>().enabled = false;
        rigidBody.velocity = Vector2.zero;
        rigidBody.angularVelocity = 0.1f;
        
        
        yield return new WaitUntil(() => (Mathf.Abs(transform.eulerAngles.z % 90) < 2f || Mathf.Abs(transform.eulerAngles.z % 90) > 88));

        rigidBody.isKinematic = true;
        rigidBody.velocity = Vector2.zero;
        rigidBody.angularVelocity = 0f;

        transform.eulerAngles = new Vector3(0, 0, Mathf.Round(transform.eulerAngles.z / 90) * 90);
        transform.position = new Vector3(transform.position.x, -3.5f + GetHeight() / 2, 1);

        towerJoint.enabled = false;
    }

    private IEnumerator SnapToTower(Tower otherTower)
    {
        yield return new WaitUntil(() => (Mathf.Approximately(rigidBody.velocity.magnitude, 0f) && Mathf.Approximately(rigidBody.angularVelocity, 0f)));
        this.tower = otherTower;
        currentCoroutine = null;
        base.OnMouseUp();
    }

    private IEnumerator ReleaseFromFoundation()
    {
        rigidBody.isKinematic = false;
        GetComponent<Collider2D>().enabled = false;
        yield return new WaitUntil(() => transform.position.y >= (-3.45f + GetHeight()/2));
        GetComponent<Collider2D>().enabled = true;
    }
    #endregion
}