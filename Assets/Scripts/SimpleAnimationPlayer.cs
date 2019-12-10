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
    /// <summary>
    /// Starts playing an animation corresponding with the given animation name.
    /// </summary>
    /// <param name="animationName">Name of the animation you want to start playing.</param>
    public void PlayAnimation(string animationName)
    {
        animator.Play(animationName);
    }
    #endregion
}