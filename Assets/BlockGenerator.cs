using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockGenerator : MonoBehaviour
{
    #region fields
    [SerializeField]
    private Vector2 spawnPosMinMaxX;
    [SerializeField]
    private Vector2 spawnPosMinMaxY;
    [SerializeField]
    private GameObject prefab;
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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="playerId"></param>
    /// <param name="ideaTitle"></param>
    public void SpawnBlock(int playerId, string ideaTitle)
    {

    }
    #endregion
}
