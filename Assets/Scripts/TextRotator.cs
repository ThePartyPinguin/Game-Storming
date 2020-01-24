using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextRotator : MonoBehaviour
{
    #region fields
    [SerializeField]
    private Transform parent = default;
    [SerializeField]
    private Transform text = default;
    [SerializeField]
    private float rotationLimit = 0.70f;
    #endregion

    #region methods
    // Update is called once per frame
    void Update()
    {
        this.text.transform.localRotation = Mathf.Abs(this.parent.rotation.normalized.z) >= rotationLimit ? 
            Quaternion.Euler(0, 0, 180) : 
            Quaternion.Euler(0, 0, 0);
    }
    #endregion
}