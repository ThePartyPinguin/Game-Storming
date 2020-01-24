using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChanger : MonoBehaviour
{
    Color startColor, highlightColor;
    Color currentColor;
    SpriteRenderer myRenderer;

    bool isHighlighted;
    bool decreasingAlpha;
    public int coint;

    public bool quickity;

    void Awake()
    {
        myRenderer = GetComponent<SpriteRenderer>();
        coint = 0;
        quickity = false;
    }

    void Update()
    {
        //if (isHighlighted)
        //{
        //    if (myRenderer.color == highlightColor)
        //    {
        //        currentColor = startColor;
        //        Debug.Log("first part");
        //    }
        //    else if (myRenderer.color == startColor)
        //    {
        //        currentColor = highlightColor;
        //        Debug.Log("bottom part");
        //    }
        //    myRenderer.color = Color.Lerp(myRenderer.color, currentColor, Mathf.PingPong(Time.deltaTime, 10)); // Mathf.PingPong lmao what the fuck
        //    myRenderer.color = Color.Lerp(myRenderer.color, currentColor, 0.05f);
        //}


        // wtf lmao
        if (quickity)
        {
            Quickity();
            quickity = false;
        }

        if (isHighlighted)
        {
            if (myRenderer.color.a < highlightColor.a)
            {
                decreasingAlpha = false;
                coint = 0;
            }
            else if (myRenderer.color.a == startColor.a)
            {
                decreasingAlpha = true;
                coint = 0;
            }

            if (decreasingAlpha)
            {
                myRenderer.color -= new Color(startColor.r * 0f, startColor.g * 0f, startColor.b * 0f, startColor.a * 0.01f);
                coint++;
            }
            else
            {
                myRenderer.color += new Color(startColor.r * 0f, startColor.g * 0f, startColor.b * 0f, startColor.a * 0.01f);
                coint++;
            }
        }
        //Debug.Log("Coint " + coint);
    }

    public void GetColor(Color c)
    {
        startColor = c;
        highlightColor = new Color(startColor.r * 1f, startColor.g * 1f, startColor.b * 1f, startColor.a * 0.35f);
        currentColor = startColor;
        decreasingAlpha = true;
    }

    void Quickity()
    {
        int ttt = 0;
        for (int i = 0; i <= coint + 1; i++)
        {
            Debug.Log(ttt++);
            if (decreasingAlpha)
                myRenderer.color -= new Color(startColor.r * 0f, startColor.g * 0f, startColor.b * 0f, startColor.a * 0.01f);
            else
                myRenderer.color += new Color(startColor.r * 0f, startColor.g * 0f, startColor.b * 0f, startColor.a * 0.01f);
        }
    }

    public void Toggle(bool t)
    {
        //if (!isHighlighted)
        //{
        //    quickity = t;
        //}
        isHighlighted = t;
        if (!t)
        {
            myRenderer.color = startColor;
        }
    }
}
