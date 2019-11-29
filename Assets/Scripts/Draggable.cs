using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public abstract class Draggable : MonoBehaviour
{
    private bool isDragged;
    private int dragId;

    private TargetJoint2D dragJoint;

    private void Awake()
    {
        dragJoint = gameObject.GetComponent<TargetJoint2D>();
    }

    public virtual void OnMouseDown()
    {
        dragJoint.enabled = true;
    }

    private void OnMouseDrag()
    {
        dragJoint.target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        GetComponent<Rigidbody2D>().angularVelocity = 0f;
    }

    private void OnMouseUp()
    {
        dragJoint.enabled = false;
    }
}
