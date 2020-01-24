using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockWatcher : MonoBehaviour
{
    private Dictionary<string, Block> ideas;

    private void Start()
    {
        ideas = new Dictionary<string, Block>();
    }

    /// <summary>
    /// Adds the provided idea to the list of this watcher
    /// </summary>
    /// <param name="newIdeaBlock"></param>
    public void AddIdea(Block newIdeaBlock)
    {
        ideas.Add(newIdeaBlock.Idea, newIdeaBlock);
    }

    /// <summary>
    /// Checks if a block with the provided idea already exists.
    /// </summary>
    /// <param name="idea">The text of the idea</param>
    /// <returns>
    /// The existing idea block if there is one.
    /// null if there is no block with the provided idea yet.
    /// </returns>
    public Block CheckIdea(string idea)
    {
        if (ideas.ContainsKey(idea))
        {
            return ideas[idea];
        }
        else
        {
            return null;
        }
    }
}
