using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour, IDraggable
{
    #region fields
    [SerializeField]
    string idea;
    [SerializeField]
    Tower tower;

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
