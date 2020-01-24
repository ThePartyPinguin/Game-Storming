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

    private bool _isCountingDown;
    #endregion

    #region methods
    /// <summary>
    /// Resets the timer to its initial value and starts the countdown again.
    /// </summary>
    public void ResetTimer()
    {
        timer = countdownTime;
        if (!_isCountingDown)
            StartCoroutine(CountDownRoutine());
    }

    private IEnumerator CountDownRoutine()
    {
        _isCountingDown = true;

        while (timer > 0)
        {
            timer -= 0.1f;
            countdownTick.Invoke(countdownTime, timer);
            yield return new WaitForSeconds(0.1f);
        }

        countdownCompleted.Invoke();
        _isCountingDown = false;
    }

    #endregion
}

[System.Serializable]
public class ProgressUnityEvent : UnityEvent<int, float> {}