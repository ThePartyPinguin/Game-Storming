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
    private SpringJoint2D blockJoint;

    Participant owner;

    #endregion

    #region methods
    private void Start()
    {
        blockJoint = GetComponent<SpringJoint2D>();
        tower = null;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Foundation") && tower == null)
        {
            Tower newTower = new Tower(this);
            this.tower = newTower;
            //FixedJoint2D towerJoint = gameObject.AddComponent<FixedJoint2D>();
            blockJoint.enabled = true;
            blockJoint.connectedBody = collision.rigidbody;
        }
        else if (collision.gameObject.CompareTag("Block") && false)
        {
            blockJoint.enabled = true;
            blockJoint.connectedBody = collision.rigidbody;
        }
    }

    public string GetIdea()
    {
        return this.idea;
    }

    public void DetachFromTower()
    {
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
    #endregion
}
