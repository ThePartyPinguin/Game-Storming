using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextRotator : MonoBehaviour
{
    #region fields
    [SerializeField]
    private Transform parent, text;
    #endregion

    #region methods
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (this.parent.rotation.normalized.z >= 0.70f || this.parent.rotation.normalized.z <= -0.70f)
        {
            this.text.transform.localRotation = Quaternion.Euler(0, 0, 180);
        }
        else
        {
            this.text.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
    }
    #endregion
}
