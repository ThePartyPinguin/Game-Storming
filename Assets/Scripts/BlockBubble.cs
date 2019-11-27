using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockBubble : MonoBehaviour
{
    #region fields
    [SerializeField]
    private Block block;
    #endregion

    #region methods
    // Start is called before the first frame update
    void Start()
    {
        block.GetComponent<Rigidbody2D>().Sleep();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    #endregion
}
