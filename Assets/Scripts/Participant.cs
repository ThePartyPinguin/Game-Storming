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
    public Participant(int _id, string _name, Color _color)
    {
        blocks = new List<Block>();
        this.id = _id;
        this.name = _name;
        this.color = _color;
        this.score = 0;
    }

    public void addBlock(Block b)
    {
        blocks.Add(b);
    }

    public Color GetColor()
    {
        return this.color;
    }

    public override string ToString()
    {
        return name;
    }

    public void AddPoints(int amount)
    {
        this.score += amount;
    }
    #endregion
}

