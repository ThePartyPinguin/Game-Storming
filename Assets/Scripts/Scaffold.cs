using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scaffold : MonoBehaviour
{
    #region fields
    [SerializeField]
    private float timer = 1f;

    private bool firstTap;
    private float lastTap;
    private SpriteRenderer spriteRenderer;
    #endregion

    #region methods
    private void Start()
    {
        this.spriteRenderer = this.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
            //check with raycast to see what object is hit
            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
            //if the object is the same, pass through to the rest of the method, else skip.
            if (hit.collider == this.GetComponent<BoxCollider2D>())
            {
                if (firstTap)
                {
                    firstTap = false;
                    if (!CheckDeleteAllowed())
                    {
                        DestroyScaffold();
                    }
                }
                else
                {
                    this.firstTap = true;
                    this.lastTap = timer;
                }
            }
        }
        //simple timer to check time between taps. if the alloted time has passed after the lasttap it will set firsttap back to false
        this.lastTap -= Time.deltaTime;
        if(this.lastTap <= 0f)
        {
            firstTap = false;
        }

    }

    /// <summary>
    /// Method that checks if deleting this scaffolding is allowed, it checks to see if theres a object on top it. if a object is found then the delete is rejected.
    /// </summary>
    /// <returns>bool if the delete is allowed.</returns>
    bool CheckDeleteAllowed()
    {
        Bounds b = spriteRenderer.bounds;
        Vector2 var = new Vector2(b.center.x - b.extents.x, (this.transform.position.y + b.extents.y * 1.2f));
        RaycastHit2D hit = Physics2D.Raycast(var, Vector2.right, b.size.x);
        return hit.collider != null;
    }

    /// <summary>
    /// Simple method that deletes the scaffold object.
    /// </summary>
    void DestroyScaffold()
    {
        Destroy(this.gameObject);
    }
    #endregion
}
