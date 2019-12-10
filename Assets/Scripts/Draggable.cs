﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public abstract class Draggable : MonoBehaviour
{
    #region fields
    private bool isDragged;
    private int dragId;

    private TargetJoint2D dragJoint;

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
        dragJoint = gameObject.GetComponent<TargetJoint2D>();
    }

    public virtual void OnMouseDown()
    {
        dragJoint.enabled = true;
        onDragDown.Invoke();
    }

    private void OnMouseDrag()
    {
        dragJoint.target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        GetComponent<Rigidbody2D>().angularVelocity = 0f;
        onDrag.Invoke();
    }

    protected void OnMouseUp()
    {
        dragJoint.enabled = false;
        onDragUp.Invoke();
    }
    #endregion
}
