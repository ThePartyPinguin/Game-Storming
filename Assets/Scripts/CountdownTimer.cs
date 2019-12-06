using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CountdownTimer : MonoBehaviour
{
    #region fields
    [SerializeField]
    private int countdownTime;
    private float timer;

    [SerializeField]
    private ProgressUnityEvent countdownTick;
    [SerializeField]
    private UnityEvent countdownCompleted;
    #endregion

    #region methods

    public void ResetTimer()
    {
        CancelInvoke();
        timer = countdownTime;
        InvokeRepeating("CountDown", 0, 0.1f);
    }

    private void CountDown()
    {
        //Subtract 0.1 seconds from timer
        timer -= 0.1f;
        countdownTick.Invoke(countdownTime, timer);

        if (timer <= 0)
        {
            CancelInvoke();
            countdownCompleted.Invoke();
        }
    }
    #endregion
}

[System.Serializable]
public class ProgressUnityEvent : UnityEvent<int, float> {}