﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : Draggable
{
    #region fields
    [SerializeField]
    private string idea;
    [SerializeField]
    private Tower tower;

    Participant owner;

    #endregion

    #region methods
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setParticipant(Participant p )
    {
        this.owner = p;
    }

    public void setIdea(string _idea)
    {
        this.idea = _idea;
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
