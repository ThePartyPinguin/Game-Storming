using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    #region fields
    [SerializeField]
    private Vector2[] mousePosition;
    [SerializeField]
    private bool mouseDown;
    [SerializeField]
    private List<UniqueInput> inputs;
    #endregion

    #region methods
    // Start is called before the first frame update
    void Start()
    {

    }

    private void FixedUpdate()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
            if (hit.collider != null)
            {
                Debug.Log(hit.collider.gameObject.name);
                
            }
        }
    }
    #endregion
}
