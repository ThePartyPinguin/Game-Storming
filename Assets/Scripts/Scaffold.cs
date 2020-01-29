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

    private Vector2 actualSize;
    #endregion

    #region methods
    private void Start()
    {
        var spriteRenderer = this.GetComponent<SpriteRenderer>();
        actualSize = new Vector2((spriteRenderer.size.x + 0.2f) / 4f, (spriteRenderer.size.y + 0.2f) / 4f);

        InvokeRepeating("CheckDeleteAllowed", timer, timer);
    }


    /// <summary>
    /// Returns the current height of the block, taking into account the rotation.
    /// </summary>
    public float GetHeight()
    {
        float functionalRotation = (transform.eulerAngles.z % 180);
        bool isRotated = 45 <= functionalRotation && functionalRotation < 135;
        return (isRotated ? actualSize.x : actualSize.y);
    }

    /// <summary>
    /// Returns the current width of the block, taking into account the rotation.
    /// </summary>
    public float GetWidth()
    {
        float functionalRotation = (transform.eulerAngles.z % 180);
        bool isRotated = 45 <= functionalRotation && functionalRotation < 135;
        return (isRotated ? actualSize.y : actualSize.x);
    }

    /// <summary>
    /// Checks whether or not another block lies on top of this block
    /// </summary>
    /// <returns>True if another block lies on top of this block, false if not</returns>
    public bool IsOtherBlockOnTop()
    {
        var checkHeight = 0.3f;
        var checkWidthCutOff = 0.01f;

        var checkBoxCenter = new Vector2(transform.position.x, transform.position.y + (GetHeight() / 2) + checkHeight);
        var checkBoxHalfExtents = new Vector3(GetWidth() / 2, 0.1f);
        var checkBoxCornerA = new Vector2(checkBoxCenter.x - checkBoxHalfExtents.x + checkWidthCutOff, checkBoxCenter.y - checkBoxHalfExtents.y);
        var checkBoxCornerB = new Vector2(checkBoxCenter.x + checkBoxHalfExtents.x - checkWidthCutOff, checkBoxCenter.y + checkBoxHalfExtents.y);
        Debug.DrawLine(checkBoxCornerA, checkBoxCornerB, Color.cyan, 1, false);
        var hit = Physics2D.OverlapArea(checkBoxCornerA, checkBoxCornerB);
        if (hit && hit.gameObject != gameObject && (hit.gameObject.GetComponent<Block>() || hit.gameObject.GetComponent<Scaffold>()) )
        {
            return true;
        }
        return false;
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
