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
    #endregion

    #region methods
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }


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
            blockbubble.GetComponentInChildren<Block>().setParticipant(owner);
            blockbubble.GetComponentInChildren<Block>().setIdea(ideaTitle);
            owner.addBlock(blockbubble.GetComponentInChildren<Block>());
            Debug.Log("[BlockGenerator.SpawnBlock] Block Created: (Owner: " + owner.ToString() + ") (BlockTitle: " + ideaTitle + ")");
        }
    }

    /// <summary>
    /// Generates a random Vector3 spawnlocation based on the minimum and maximum x,y values defined in spawnPosMinMaxX and spawnPosMinMaxY.
    /// </summary>
    /// <returns>
    /// Vector3 With a randomly generated x and y value.
    /// Returns a Vector3(-999999, -999999) on error combined with a console log.
    /// </returns>
    private Vector3 GenerateSpawnlocation()
    {
        //blame marco if its unreadable
        if (spawnPosMinMaxX != null && spawnPosMinMaxY != null)
        {
            if (spawnPosMinMaxX.x < spawnPosMinMaxX.y && spawnPosMinMaxY.x < spawnPosMinMaxY.y)
            {
                return new Vector3(Random.Range(spawnPosMinMaxX.x, spawnPosMinMaxX.y), Random.Range(spawnPosMinMaxY.x, spawnPosMinMaxY.y));
            }
        }
        Debug.Log("[BlockGenerator.GenerateSpawnLocation] : Error generating spawn location.");
       
        return new Vector3(-999999, -999999);
    }
    #endregion
}
