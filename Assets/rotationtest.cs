using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotationtest : MonoBehaviour
{
    Bounds b;
    public float x;
    public float y;

    // Start is called before the first frame update
    void Start()
    {
        b = this.GetComponent<SpriteRenderer>().bounds;
    }

    // Update is called once per frame
    void Update()
    {
        b = this.GetComponent<SpriteRenderer>().bounds;
    }

    private void OnDrawGizmos()
    {
        
        Gizmos.DrawCube(b.center, new Vector3(b.size.x * x, b.size.y * y));
        Vector2 var = new Vector2(b.center.x - b.extents.x, (b.extents.y * 1.2f));
        Gizmos.DrawCube(var, new Vector2(0.2f, 0.2f));
    }
}
