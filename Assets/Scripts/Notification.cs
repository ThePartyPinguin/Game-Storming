using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Notification : MonoBehaviour
{

    public Animator popUp;


    
    public void PoppingUp()
    {
        popUp.Play("PopUp");
        //panel.SetActive(false);
    }

    public void DissappearNotification()
    {
        popUp.Play("ClosePopUp");
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    private void PanelActive()
    {
       // panel.SetActive(true);
    }
}

