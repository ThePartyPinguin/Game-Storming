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
        float functionalRotation = (firstBlock.transform.eulerAngles.z % 180);
        bool isHorizontal = !(45 <= functionalRotation && functionalRotation < 135);
        this.height = isHorizontal ? firstBlock.transform.localScale.x : firstBlock.transform.localScale.y;
        this.blocks = new List<Block>();
        blocks.Add(firstBlock);
    }
    #endregion

    #region methods

    #endregion
}
