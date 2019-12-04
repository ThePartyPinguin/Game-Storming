using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SimpleAnimationPlayer : MonoBehaviour
{
    #region fields
    [SerializeField]
    private Animator animator;
    #endregion

    #region methods
    public void PlayAnimation(string animationName)
    {
        animator.Play(animationName);
    }
    #endregion
}