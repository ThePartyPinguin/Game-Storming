using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class TutorialInfoButton : MonoBehaviour
{
    public bool isOpened;
    [SerializeField]
    private GameObject instructions = default;
    [SerializeField]
    public TutorialManager tutorialManager = default;
    [SerializeField]
    public UnityEvent Highlighter = default;

    private void Start()
    {
        isOpened = false;
    }

    void OnMouseDown()
    {
        Highlight();
        Highlighter.Invoke();
        //Highlight();
    }

    public virtual void Highlight() {
        isOpened = !isOpened;
        instructions.SetActive(isOpened);
    }

    public virtual void HighlightControl(bool b)
    {
        isOpened = b;
        instructions.SetActive(isOpened);
    }
}
