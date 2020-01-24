using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlatformCollision : MonoBehaviour
{

    List<GameObject> onGround;
    [SerializeField]
    UnityEvent tutorialEvents;

    void Start()
    {
        onGround = new List<GameObject>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        foreach(GameObject g in onGround)
        {
            if (g == collision.gameObject)
                return;
        }
        onGround.Add(collision.gameObject);
        ShowBuildingTutorialButton();
    }

    void ShowBuildingTutorialButton()
    {
        if(onGround.Count == 2)
        {
           // tutorialEvents.Invoke();
            Debug.Log("We did it");
        }
    }
}
