using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockBubble : MonoBehaviour
{
    #region fields
    [SerializeField]
    private Block block;

    private float timer;
    private Vector2 startposition;
    #endregion

    #region methods
    // Start is called before the first frame update
    void Start()
    {
        this.startposition = this.transform.position;
    }

    private void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;
        float x = Mathf.Cos(timer);
        float y = Mathf.Sin(timer);

        this.transform.parent.position = startposition + new Vector2(x, y);
    }

    #endregion
}
