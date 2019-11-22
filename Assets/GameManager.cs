using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region fields
    [SerializeField]
    float buildingTime;

    List<Participant> participants;
    Participant currentBuilder;
    float timer;
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
    #endregion
}
