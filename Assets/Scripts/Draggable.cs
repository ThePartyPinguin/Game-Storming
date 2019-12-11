using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public abstract class Draggable : MonoBehaviour
{
    #region fields
    protected bool isDragged;
    private int dragId;
    private int originalLayer;

    protected TargetJoint2D dragJoint;

    [SerializeField]
    private UnityEvent onDragDown;
    [SerializeField]
    private UnityEvent onDrag;
    [SerializeField]
    private UnityEvent onDragUp;
    #endregion

    #region methods
    private void Awake()
    {
        //Caching values
        originalLayer = gameObject.layer;
        dragJoint = gameObject.GetComponent<TargetJoint2D>();
    }

    private void Update()
    {
        if (isDragged)
        {
            dragJoint.target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
    }

    public virtual void OnMouseDown()
    {
        gameObject.layer = 9;
        dragJoint.enabled = true;
        onDragDown.Invoke();
    }

    private void OnMouseDrag()
    {
        //dragJoint.target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        GetComponent<Rigidbody2D>().angularVelocity = 0f;
        onDrag.Invoke();
    }

    protected virtual void OnMouseUp()
    {
        gameObject.layer = originalLayer;
        dragJoint.enabled = false;
        onDragUp.Invoke();
    }
    #endregion
}
