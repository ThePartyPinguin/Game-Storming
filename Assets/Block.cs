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
    private HingeJoint2D towerJoint;

    Participant owner;

    #endregion

    #region methods
    private void Start()
    {
        towerJoint = GetComponent<HingeJoint2D>();
        rigidBody = GetComponent<Rigidbody2D>();
        tower = null;
        isConnected = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isConnected) { Debug.Log("Shouldn't happen :c " + Time.time);  return; }
        if (collision.gameObject.CompareTag("Foundation") && tower == null)
        {
            Debug.Log("Tower created" + Time.time);
            Tower newTower = new Tower(this);
            this.tower = newTower;
            this.isConnected = true;
            
            towerJoint.enabled = true;
            Vector2 contactPoint = transform.InverseTransformPoint(collision.GetContact(0).point);
            towerJoint.anchor = new Vector2(
                contactPoint.x < 1 ? Mathf.Max(contactPoint.x, -0.5f) : Mathf.Min(contactPoint.x, 0.5f),
                contactPoint.y < 1 ? Mathf.Max(contactPoint.y, -0.5f) : Mathf.Min(contactPoint.y, 0.5f));
            towerJoint.connectedBody = collision.rigidbody;
            StartCoroutine(SnapToFoundation());
            base.OnMouseUp();
        }
        else if (collision.gameObject.CompareTag("Block") && collision.gameObject.GetComponent<Block>()) //Extra check for if it's done
        {
            Debug.Log("HitBlock" + Time.time);
            Block otherBlock = collision.gameObject.GetComponent<Block>();
            //base.OnMouseUp();
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
        StartCoroutine(ReleaseFromFoundation());
        tower = null;
        this.isConnected = false;
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
        yield return new WaitUntil(() => transform.position.y >= (-3.45f + GetHeight()/2));
        GetComponent<Collider2D>().enabled = true;
    }
    #endregion
}