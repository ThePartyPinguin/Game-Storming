using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : Draggable
{
    #region fields
    [SerializeField]
    private string idea;
    [SerializeField]
    private Tower tower;
    private Rigidbody2D rigidBody;
    private bool isConnected;
    private SpringJoint2D blockJoint;
    private HingeJoint2D towerJoint;

    Participant owner;

    #endregion

    #region methods
    private void Start()
    {
        blockJoint = GetComponent<SpringJoint2D>();
        towerJoint = GetComponent<HingeJoint2D>();
        rigidBody = GetComponent<Rigidbody2D>();
        tower = null;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Foundation") && tower == null)
        {
            Tower newTower = new Tower(this);
            this.tower = newTower;
            
            towerJoint.enabled = true;
            Vector2 contactPoint = transform.InverseTransformPoint(collision.GetContact(0).point);
            towerJoint.anchor = new Vector2(
                contactPoint.x < 1 ? Mathf.Max(contactPoint.x, -0.5f) : Mathf.Min(contactPoint.x, 0.5f),
                contactPoint.y < 1 ? Mathf.Max(contactPoint.y, -0.5f) : Mathf.Min(contactPoint.y, 0.5f));
            towerJoint.connectedBody = collision.rigidbody;
            StartCoroutine(SnapToFoundation());
            base.OnMouseUp();
        }
        else if (collision.gameObject.CompareTag("Block") && collision.gameObject.GetComponent<Block>())
        {
            Block otherBlock = collision.gameObject.GetComponent<Block>();
            blockJoint.enabled = true;
            Vector2 contactPoint = transform.InverseTransformPoint(collision.GetContact(0).point);
            blockJoint.anchor = new Vector2(
                contactPoint.x < 1 ? Mathf.Max(contactPoint.x, -0.5f) : Mathf.Min(contactPoint.x, 0.5f),
                contactPoint.y < 1 ? Mathf.Max(contactPoint.y, -0.5f) : Mathf.Min(contactPoint.y, 0.5f));
            blockJoint.connectedBody = collision.rigidbody;
            base.OnMouseUp();
            this.tower = otherBlock.GetTower();
        }
    }

    public string GetIdea()
    {
        return this.idea;
    }

    public float GetHeight()
    {
        float functionalRotation = (transform.eulerAngles.z % 180);
        bool isHorizontal = 45 <= functionalRotation && functionalRotation < 135;
        return (isHorizontal ? transform.localScale.x : transform.localScale.y);
    }

    public Tower GetTower()
    {
        return this.tower;
    }

    public void DetachFromTower()
    {
        if (!rigidBody.isKinematic) { return; }
        //blockJoint.enabled = false;
        StartCoroutine(ReleaseFromFoundation());
        tower = null;
    }

    public int CalculateScore()
    {
        throw new System.NotImplementedException();
    }

    public int dragId()
    {
        throw new System.NotImplementedException();
    }

    public bool isDragged()
    {
        throw new System.NotImplementedException();
    }

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

    private IEnumerator ReleaseFromFoundation()
    {
        rigidBody.isKinematic = false;
        GetComponent<Collider2D>().enabled = false;
        yield return new WaitUntil(() => transform.position.y >= (-2.5f + GetHeight()/2));
        GetComponent<Collider2D>().enabled = true;
    }
    #endregion
}