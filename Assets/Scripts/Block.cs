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
    [SerializeField]
    private TextMeshPro textVisual;

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
        if(this.textVisual != null)
        {
            this.textVisual.text = idea;
        }
    }

    public void DetachAndDestroy()
    {
        if (this.transform.parent != null)
        {
            GameObject go = this.transform.parent.gameObject;
            this.transform.SetParent(null);
            Destroy(go);
        }
    }

    public override void OnMouseDown()
    {
         this.DetachAndDestroy();
            base.OnMouseDown();
        
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
