using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniqueInput : MonoBehaviour
{
    #region fields
    [SerializeField]
    private Vector2 position;
    [SerializeField]
    private int inputId;
    [SerializeField]
    private Draggable dragObject;
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

    // OnDestroy is called before the destruction of an object
    public void OnDestroy()
    {
        
    }
    #endregion
}
