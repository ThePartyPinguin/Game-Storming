using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockBubble : MonoBehaviour
{
    #region fields
    private float timer;
    private Vector2 startposition;
    private Transform parentTransform;
    #endregion

    #region methods
    // Start is called before the first frame update
    void Start()
    {
        //Caching values
        this.startposition = this.transform.position;
        this.parentTransform = this.transform.parent;
    }

    private void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;
        float x = Mathf.Cos(timer);
        float y = Mathf.Sin(timer);

        parentTransform.position = startposition + new Vector2(x, y);
    }
    #endregion
}
