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
    protected int originalLayer;

    private Rigidbody2D rigidBody;
    protected TargetJoint2D dragJoint;
    private Camera cam;
    protected float lowestBoundary;

    [SerializeField]
    private UnityEvent onDragDown = default;
    [SerializeField]
    private UnityEvent onDrag = default;
    [SerializeField]
    private UnityEvent onDragUp = default;
    #endregion

    #region methods
    protected void Start()
    {
        //Caching values
        originalLayer = gameObject.layer;
        rigidBody = GetComponent<Rigidbody2D>();
        dragJoint = gameObject.GetComponent<TargetJoint2D>();
        cam = Camera.main;

        
        //lowestBoundary = GameManager.Instance.FoundationTop + 0.5f;
        
    }
    

    protected void Update()
    {
        if (isDragged)
        {
            var mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

            mousePos.y = Mathf.Max(mousePos.y, lowestBoundary);
            dragJoint.target = mousePos;
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
        rigidBody.angularVelocity = 0f;
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
