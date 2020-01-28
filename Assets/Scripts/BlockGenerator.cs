using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(BlockWatcher))]
public class BlockGenerator : MonoBehaviour
{
    #region fields
    [SerializeField]
    private Vector2 spawnPosMinMaxX = default; 
    [SerializeField]
    private Vector2 spawnPosMinMaxY = default;
    [SerializeField]
    private Block prefab = default;
    [SerializeField]
    private FollowEffect blockExistsEffectPrefab = default;
    [SerializeField]
    private AnimationCurve blockSizeX = default;
    [SerializeField]
    private AnimationCurve blockSizeY = default;
    [SerializeField]
    public Transform bubbleMover = default;
    [SerializeField]
    private BlockWatcher blockWatcher = default;
    #endregion

    #region methods

    /// <summary>
    /// Spawns a new block with given owner and idea in a new bubble at a random location.
    /// </summary>
    /// <param name="owner"></param>
    /// <param name="ideaText"></param>
    public void SpawnBlock(Participant owner, string ideaText)
    {
        //Parameter check
        if (owner == null || ideaText == "") { return; }

        var existingBlock = blockWatcher.CheckIdea(ideaText);

        if (!existingBlock) //The idea doesn't exist on a block yet → spawn new block
        {
            Block newBlock = Instantiate(prefab, GenerateSpawnlocation(), Quaternion.identity);
            newBlock.GetComponentsInParent<SpriteRenderer>()[0].color = owner.Color;
            newBlock.Owner = owner;
            newBlock.Idea = ideaText;
            owner.AddBlock(newBlock);
            blockWatcher.AddIdea(newBlock);

            //Variable block size based on amount of characters in the idea
            float newSizeX = blockSizeX.Evaluate(ideaText.Length);
            float newSizeY = blockSizeY.Evaluate(ideaText.Length);
            Vector2 newSize = new Vector2(newSizeX, newSizeY);
            Vector2 newColliderSize = new Vector2(newSizeX - 0.05f, newSizeY - 0.05f);

            newBlock.GetComponent<SpriteRenderer>().size = newSize;
            foreach (var collider in newBlock.GetComponents<BoxCollider2D>())
            {
                collider.size = newColliderSize;
            }
            newBlock.GetComponentInChildren<TrailRenderer>().widthMultiplier = Mathf.Min(newSizeX, newSizeY);
            newBlock.GetComponentInChildren<BlockBubble>().transform.localScale *= (Mathf.Max(newSizeX, newSizeY) * 0.75f);
            newBlock.GetComponentInChildren<RectTransform>().sizeDelta = newSize;

            newBlock.transform.parent = bubbleMover;
        }
        else
        {
            FollowEffect followEffect = Instantiate(blockExistsEffectPrefab, GenerateSpawnlocation(), Quaternion.identity);
            followEffect.SetTarget(existingBlock.transform);
            existingBlock.IncreaseImportance();
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
