using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockGenerator : MonoBehaviour
{
    #region fields
    [SerializeField]
    private Vector2 spawnPosMinMaxX; 
    [SerializeField]
    private Vector2 spawnPosMinMaxY;
    [SerializeField]
    private GameObject prefab;
    [SerializeField]
    private Transform bubbleMover;
    #endregion

    #region methods
    /// <summary>
    /// 
    /// </summary>
    /// <param name="owner"></param>
    /// <param name="ideaTitle"></param>
    public void SpawnBlock(Participant owner, string ideaTitle)
    {
        if (owner != null && ideaTitle != "")
        {
            GameObject blockbubble = Instantiate(prefab, GenerateSpawnlocation(), Quaternion.identity);
            blockbubble.GetComponentsInParent<SpriteRenderer>()[0].color = owner.GetColor();
            blockbubble.GetComponentInChildren<Block>().Participant = owner;
            blockbubble.GetComponentInChildren<Block>().Idea = ideaTitle;
            owner.addBlock(blockbubble.GetComponentInChildren<Block>());
            Debug.Log("[BlockGenerator.SpawnBlock] Block Created: (Owner: " + owner.ToString() + ") (BlockTitle: " + ideaTitle + ")");

            blockbubble.transform.parent = bubbleMover;
        }
    }

    /// <summary>
    /// updates the spawn area coordinates, only does the x coordinate
    /// </summary>
    /// <param name="position">Vector3 value representing the changed x value that needs to be updated. (can be used for y but currently isn't)</param>
    public void UpdateSpawnArea(Vector3 position)
    {
        this.spawnPosMinMaxX.x += position.x;
        this.spawnPosMinMaxX.y += position.x;
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
            return new Vector2(Random.Range(spawnPosMinMaxX.x, spawnPosMinMaxX.y), Random.Range(spawnPosMinMaxY.x, spawnPosMinMaxY.y));
        }
        Debug.LogError("[BlockGenerator.GenerateSpawnLocation] : Error generating spawn location. Defaulting to zero.");
        return Vector2.zero;
    }
    #endregion
}
