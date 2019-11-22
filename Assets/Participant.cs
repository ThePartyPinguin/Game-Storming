using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class Participant
{
    #region fields
    private int id;
    private string name;
    private Color color;
    private int score;
    private List<Block> blocks;
    #endregion

    #region methods
    public Participant(int _id, string _name)
    {
        blocks = new List<Block>();
        this.id = _id;
        this.name = _name;
    }

    public void addBlock(Block b)
    {
        blocks.Add(b);
    }

    public override string ToString()
    {
        return name;
    }
    #endregion
}

