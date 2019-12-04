using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower
{
    #region fields
    [SerializeField]
    private string idea;
    [SerializeField]
    private float height;
    [SerializeField]
    private int breakingHeight;
    [SerializeField]
    private List<Block> blocks;
    [SerializeField]
    private float pointScale = 10f;

    public Tower(Block firstBlock)
    {
        this.idea = firstBlock.GetIdea() ?? throw new ArgumentNullException(nameof(idea));
        this.height = firstBlock.GetHeight();
        this.blocks = new List<Block>();
        blocks.Add(firstBlock);
    }
    
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
    #endregion

    #region methods
    private int CalculateScore()
    {
        return Mathf.FloorToInt(pointScale / this.blocks.Count);
    }
    #endregion
}
