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

    private float testBoundOrigin;
    private float testBoundWidth;
    private float testBoundHeight;
    #endregion

    #region methods
    private void Start()
    {
        spriteRenderer = this.GetComponent<SpriteRenderer>();

        Bounds b = spriteRenderer.bounds;
        testBoundOrigin = b.center.x - b.extents.x;
        testBoundWidth = b.size.x;
        testBoundHeight = b.extents.y * 1.2f;

        InvokeRepeating("CheckDeleteAllowed", 1, 1);
    }

    //// Update is called once per frame
    //void Update()
    //{
    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
    //        //check with raycast to see what object is hit
    //        RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
    //        //if the object is the same, pass through to the rest of the method, else skip.
    //        if (hit.collider == this.GetComponent<BoxCollider2D>())
    //        {
    //            if (firstTap)
    //            {
    //                firstTap = false;
    //                if (!CheckDeleteAllowed())
    //                {
    //                    DestroyScaffold();
    //                }
    //            }
    //            else
    //            {
    //                this.firstTap = true;
    //                this.lastTap = timer;
    //            }
    //        }
    //    }
    //    //simple timer to check time between taps. if the alloted time has passed after the lasttap it will set firsttap back to false
    //    this.lastTap -= Time.deltaTime;
    //    if(this.lastTap <= 0f)
    //    {
    //        firstTap = false;
    //    }

    //}


    /// <summary>
    /// Returns the current height of the block, taking into account the rotation.
    /// </summary>
    public float GetHeight()
    {
        float functionalRotation = (transform.eulerAngles.z % 180);
        bool isRotated = 45 <= functionalRotation && functionalRotation < 135;
        return (isRotated ? spriteRenderer.size.x : spriteRenderer.size.y);
    }

    /// <summary>
    /// Returns the current width of the block, taking into account the rotation.
    /// </summary>
    public float GetWidth()
    {
        float functionalRotation = (transform.eulerAngles.z % 180);
        bool isRotated = 45 <= functionalRotation && functionalRotation < 135;
        return (isRotated ? spriteRenderer.size.y : spriteRenderer.size.x);
    }

    /// <summary>
    /// Checks whether or not another block lies on top of this block
    /// </summary>
    /// <returns>True if another block lies on top of this block, false if not</returns>
    public bool IsOtherBlockOnTop()
    {
        var checkHeight = 0.75f;
        var checkWidthCutOff = 0.1f;

        var checkBoxCenter = new Vector2(transform.position.x, transform.position.y + (GetHeight() / 2) + checkHeight);
        var checkBoxHalfExtents = new Vector3(GetWidth() / 2, GetHeight() / 10);
        var checkBoxCornerA = new Vector2(checkBoxCenter.x - checkBoxHalfExtents.x + checkWidthCutOff, checkBoxCenter.y - checkBoxHalfExtents.y);
        var checkBoxCornerB = new Vector2(checkBoxCenter.x + checkBoxHalfExtents.x - checkWidthCutOff, checkBoxCenter.y + checkBoxHalfExtents.y);
        Debug.DrawLine(checkBoxCornerA, checkBoxCornerB, Color.blue, 2, false);
        return (Physics2D.OverlapArea(checkBoxCornerA, checkBoxCornerB) != null);
    }

    /// <summary>
    /// Method that checks if deleting this scaffolding is allowed, it checks to see if theres a object on top it. if a object is found then the delete is rejected.
    /// </summary>
    /// <returns>bool if the delete is allowed.</returns>
    bool CheckDeleteAllowed()
    {
        //Vector2 testBound = new Vector2(testBoundOrigin, this.transform.position.y + testBoundHeight);
        //RaycastHit2D hit = Physics2D.Raycast(testBound, Vector2.right, testBoundWidth);
        //return hit.collider != null;

        if (!IsOtherBlockOnTop())
        {
            DestroyScaffold();
            return true;
        }
        else
        {
            return false;
        }
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
