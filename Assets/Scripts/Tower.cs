using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    #region fields
    [SerializeField]
    private string idea;
    [SerializeField]
    private int height;
    [SerializeField]
    private int breakingHeight;
    [SerializeField]
    private List<Block> blocks;
    [SerializeField]
    private float pointScale = 10f;
    #endregion

    /// <summary>
    /// Pointsystem...
    /// points scale depending on amount of blocks present in tower, less blocks equals more points.
    /// </summary>


    #region methods
    public int AddBlock(Block block)
    {
        this.blocks.Add(block);
        return CalculateScore();
    }

    private int CalculateScore()
    {
        return Mathf.FloorToInt(pointScale / this.blocks.Count);
    }
    #endregion
}
