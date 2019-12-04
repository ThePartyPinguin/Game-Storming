using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region fields
    [SerializeField]
    public float buildingTime;

    private List<Participant> participants;
    private Participant currentBuilder;
    private float timer;
    #endregion

    #region methods
    #endregion
}
