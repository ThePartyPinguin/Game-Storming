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

    public Tower(Block firstBlock)
    {
        this.idea = firstBlock.GetIdea() ?? throw new ArgumentNullException(nameof(idea));
        this.height = firstBlock.GetHeight();
        this.blocks = new List<Block>();
        blocks.Add(firstBlock);
    }
    #endregion

    #region methods

    #endregion
}
