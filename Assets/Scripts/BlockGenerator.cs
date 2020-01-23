using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BlockGenerator : MonoBehaviour
{
    #region fields
    [SerializeField]
    private Vector2 spawnPosMinMaxX; 
    [SerializeField]
    private Vector2 spawnPosMinMaxY;
    [SerializeField]
    private Block prefab;
    [SerializeField]
    private AnimationCurve blockSizeX;
    [SerializeField]
    private AnimationCurve blockSizeY;
    [SerializeField]
    public Transform bubbleMover;
    #endregion

    #region methods

    /// <summary>
    /// Spawns a new block with given owner and idea in a new bubble at a random location.
    /// </summary>
    /// <param name="owner"></param>
    /// <param name="ideaTitle"></param>
    public void SpawnBlock(Participant owner, string ideaTitle)
    {
        if (owner != null && ideaTitle != "")
        {
            Block blockbubble = Instantiate(prefab, GenerateSpawnlocation(), Quaternion.identity);
            blockbubble.GetComponentsInParent<SpriteRenderer>()[0].color = owner.Color;
            blockbubble.Owner = owner;
            blockbubble.Idea = ideaTitle;
            owner.AddBlock(blockbubble);

            //Variable block size based on amount of characters in the idea
            float newSizeX = blockSizeX.Evaluate(ideaTitle.Length);
            float newSizeY = blockSizeY.Evaluate(ideaTitle.Length);
            Vector2 newSize = new Vector2(newSizeX, newSizeY);
            Vector2 newColliderSize = new Vector2(newSizeX - 0.05f, newSizeY - 0.05f);

            return;
            blockbubble.GetComponent<SpriteRenderer>().size = newSize;
            foreach (var collider in blockbubble.GetComponents<BoxCollider2D>())
            {
                collider.size = newColliderSize;
            }
            blockbubble.GetComponentInChildren<TrailRenderer>().widthMultiplier = Mathf.Min(newSizeX, newSizeY);
            blockbubble.GetComponentInChildren<BlockBubble>().transform.localScale *= (Mathf.Max(newSizeX, newSizeY) * 0.75f);
            blockbubble.GetComponentInChildren<RectTransform>().sizeDelta = newSize;
            
            blockbubble.transform.parent = bubbleMover;

            IdeaLogger.LogIdea(owner, ideaTitle);
        }
    }

    /// <summary>
    /// updates the spawn area coordinates, only does the x coordinate
    /// </summary>
    /// <param name="position">Vector3 value representing the changed x value that needs to be updated. (can be used for y but currently isn't)</param>
    public void UpdateSpawnArea(Vector3 position)
    {
        //this.spawnPosMinMaxX.x += position.x;
        //this.spawnPosMinMaxX.y += position.x;
        transform.position += position;
        this.bubbleMover.position += new Vector3(position.x, this.bubbleMover.position.y, this.bubbleMover.position.z);
    }

    /// <summary>
    /// Generates a random Vector3 spawnlocation based on the minimum and maximum x,y values defined in spawnPosMinMaxX and spawnPosMinMaxY.
    /// </summary>
    /// <returns>
    /// Vector3 With a randomly generated x and y value.
    /// Returns a Vector3.zero on error combined with a console log.
    /// </returns>
    private Vector2 GenerateSpawnlocation()
    {
        if (spawnPosMinMaxX.x < spawnPosMinMaxX.y && spawnPosMinMaxY.x < spawnPosMinMaxY.y)
        {
            return new Vector2(transform.position.x + Random.Range(spawnPosMinMaxX.x, spawnPosMinMaxX.y), Random.Range(spawnPosMinMaxY.x, spawnPosMinMaxY.y));
        }
        Debug.LogError("[BlockGenerator.GenerateSpawnLocation] : Error generating spawn location. Defaulting to zero.");
        return Vector2.zero;
    }

    void OnApplicationQuit()
    {
        IdeaLogger.EndLogging();
    }

    #endregion
}
