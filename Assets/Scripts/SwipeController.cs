using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeController : MonoBehaviour
{

    #region fields
    [SerializeField]
    public bool restrictSwipeArea = false;
    [SerializeField]
    public Rigidbody2D cam;
    [SerializeField]
    public float scrollSpeed;
    [SerializeField]
    public BlockGenerator blockGen;
    [SerializeField]
    public Transform bubbleMover;

    private Vector2 touchStart;
    private Vector2 touchEnd;
    private Vector3 startPosition;
    #endregion

    #region Methods

    private void Start()
    {
        this.startPosition = this.transform.position;
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 v = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D[] hits = Physics2D.RaycastAll(new Vector2(v.x, v.y), Vector2.zero);
            if (restrictSwipeArea)
            {
                if (!foundationHit(hits))
                    return;
            }
            else
            {
                if (blockHit(hits))
                    return;
            }
            this.touchStart = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0) && this.touchStart != Vector2.zero)
        {
            this.touchEnd = Input.mousePosition;
            cam.AddForce(new Vector2(touchStart.x - touchEnd.x, touchStart.y - touchEnd.y) * scrollSpeed);
            touchStart = Vector2.zero;
        }

    }

    void LateUpdate()
    {
        Vector3 v = (this.transform.position - this.startPosition);
        blockGen.UpdateSpawnArea(v);
        foreach (BlockBubble blockBubble in bubbleMover.GetComponentsInChildren<BlockBubble>())
        {
            blockBubble.startposition += new Vector2(v.x, v.y);
        }
        this.startPosition = this.transform.position;
    }

    /// <summary>
    /// checks the list of raycast hits to see if a foundation layer was hit.
    /// </summary>
    /// <param name="hits">the list of raycast hits that needs to be checked</param>
    /// <returns>bool thats true if a foundation has been hit</returns>
    private bool foundationHit(RaycastHit2D[] hits)
    {
        if (hits.Length > 0)
        {
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].collider.gameObject.layer == 8)
                {
                    return true;
                }
            } 
        }
        return false;
    }

    /// <summary>
    /// Checks the list of raycast hits to see if a block tag has been hit.
    /// </summary>
    /// <param name="hits">the list of raycast hits that needs to be checked</param>
    /// <returns>bool thats true if a block has been hit</returns>
    private bool blockHit(RaycastHit2D[] hits)
    {
        if (hits.Length > 0)
        {
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].collider.tag == "Block")
                {
                    return true;
                }
            }
        }
        return false;
    }

    #endregion
}
