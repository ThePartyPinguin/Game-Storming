using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeController : MonoBehaviour
{

    #region fields
    [SerializeField]
    public bool restrictSwipeArea = false;
    [SerializeField]
    public float scrollSpeed;
    [SerializeField]
    public BlockGenerator blockGen;
    [SerializeField]
    public Transform bubbleMover;

    private Camera cam;
    private Rigidbody2D rb;

    private float touchStartX;
    private float touchEndX;
    private Vector3 startPosition;
    #endregion

    #region Methods

    private void Start()
    {
        cam = Camera.main;
        rb = GetComponent<Rigidbody2D>();
        this.startPosition = this.transform.position;
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 v = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            int layerMask = ~(1 << 5);
            GameObject hit = Physics2D.Raycast(new Vector2(v.x, v.y), Vector2.zero, 100, layerMask).collider.gameObject;
            if (hit && hit.layer == 8)
            {
                touchStartX = cam.ScreenToWorldPoint(Input.mousePosition).x;
            }
            return;
        }

        if (Input.GetMouseButton(0))
        {
            touchEndX = cam.ScreenToWorldPoint(Input.mousePosition).x;
            rb.AddForce(new Vector2((touchStartX - touchEndX) * scrollSpeed, 0));
            
            touchStartX = touchEndX;
        }
    }

    void LateUpdate()
    {
        //get difference in position from last update
        Vector3 v = (this.transform.position - this.startPosition);
        //update the spawn area and move the blockbubbles with the new camera position
        blockGen.UpdateSpawnArea(v);
        //move each block bubbles startposition, this needs to be done in order to prevent it from snapping back.
        var additionalPosition = new Vector2(v.x, v.y);
        foreach (BlockBubble blockBubble in bubbleMover.GetComponentsInChildren<BlockBubble>())
        {
            blockBubble.startposition += additionalPosition;
        }
        this.startPosition = this.transform.position;
    }

    #endregion
}
