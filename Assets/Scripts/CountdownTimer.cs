using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CountdownTimer : MonoBehaviour {
    public int duration;
    private float countdown;
    public Image timerVisuals;
    public TMP_Text timerText;
    public Gradient newColor;
    public Gradient alpha;
    public UnityEvent completed;


    // Use this for initialization
    void Start () {
        Reset();
        
    }

    public void Reset()
    {
        CancelInvoke();
        countdown = duration;
        InvokeRepeating("CountDown", 0, 0.1f);
    }

    public float GetCountdown()
    {
        return countdown;
    }



    private void CountDown()
    {
        countdown -= 0.1f;
        UpdateUI();
        if (countdown <= 0)
        {
            CancelInvoke();
            completed.Invoke();
        }
    }

    private void UpdateUI()
    {
        timerText.text = Mathf.Ceil(countdown).ToString();
        timerText.color = newColor.Evaluate(countdown / duration);
        timerVisuals.fillAmount = (countdown / duration);
        timerVisuals.GetComponentsInChildren<Image>()[1].color = alpha.Evaluate(countdown / duration);
    }
}
