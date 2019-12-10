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
    #endregion

    #region methods
    public Tower(Block firstBlock)
    {
        this.idea = firstBlock.Idea ?? throw new ArgumentNullException(nameof(idea));
        this.height = firstBlock.GetHeight();
        this.blocks = new List<Block>();
        blocks.Add(firstBlock);
    }

    /// <summary>
    /// Adds the given block to this tower and returns the score the block's player gets from this.
    /// </summary>
    /// <param name="block">The block to be added to this tower</param>
    /// <returns>The score that the block gives to the owning participant for being used in this tower</returns>
    public int AddBlock(Block block)
    {
        this.blocks.Add(block);
        return CalculateScore();
    }

    /// <summary>
    /// Calculates the amount of points a player gets for getting his block in this tower.
    /// Points scale depending on amount of blocks present in tower, less blocks equals more points.
    /// </summary>
    /// <returns>The score that the block gives to the owning participant for being used in this tower</returns>
    private int CalculateScore()
    {
        return Mathf.FloorToInt(pointScale / this.blocks.Count);
    }
    #endregion
}
