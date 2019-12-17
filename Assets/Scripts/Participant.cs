using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class Participant
{
    #region fields
    public Guid Id { get; }
    private string name;
    private Color color;
    private int score;
    private List<Block> blocks;
    #endregion

    #region properties
    public string Name { get { return this.name; } }

    public Color Color { get { return this.color; } }
    #endregion

    #region methods
    public Participant(Guid id, string name, Color color)
    {
        blocks = new List<Block>();
        this.Id = id;
        this.name = name;
        this.color = color;
        this.score = 0;
    }

    public void AddBlock(Block b)
    {
        blocks.Add(b);
    }

    public override string ToString()
    {
        return name;
    }

    public void AddPoints(int amount)
    {
        this.score += amount;
    }

    public Guid GetId()
    {
        return this.Id;
    }
    #endregion
}

