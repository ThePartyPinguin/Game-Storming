using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CountdownTimerUIController : MonoBehaviour
{
    [SerializeField]
    private Image timerVisuals = default;
    [SerializeField]
    private TMP_Text timerText = default;
    [SerializeField]
    private Gradient colorRange = default;
    [SerializeField]
    private Gradient alphaRange = default;

    /// <summary>
    /// Updates the timer UI to the progress of the timer.
    /// </summary>
    /// <param name="totalTime">Total duration of the counting down</param>
    /// <param name="currentTime">Time left in the countdown</param>
    public void UpdateUI(int totalTime, float currentTime)
    {
        timerText.text = Mathf.Ceil(currentTime).ToString();
        var progress = currentTime / totalTime;
        timerText.color = colorRange.Evaluate(progress);
        timerVisuals.fillAmount = progress;
        timerVisuals.GetComponentsInChildren<Image>()[1].color = alphaRange.Evaluate(progress);
    }
}
