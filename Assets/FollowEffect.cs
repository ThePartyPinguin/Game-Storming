using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowEffect : MonoBehaviour
{
    [SerializeField]
    private float completionTime = 2f;
    private float time = 0;
    [SerializeField]
    private AnimationCurve effectSpeed = default;

    private Transform target;
    private Vector2 startingPos;

    private void Start()
    {
        startingPos = transform.position;
    }

    public void SetTarget(Transform newTarget)
    {
        this.target = newTarget;
    }

    // Update is called once per frame
    void Update()
    {
        if (!target) { return; }
        float progress = time / completionTime;
        time += (Time.deltaTime * effectSpeed.Evaluate(progress));
        transform.position = Vector2.Lerp(startingPos, target.position, time / completionTime);

        if (progress < 1f)
        {
            transform.position = Vector2.Lerp(startingPos, target.position, time / completionTime);
        }
        else if (progress >= 1f && progress < 1.5f)
        {
            GetComponent<ParticleSystem>().Stop(false, ParticleSystemStopBehavior.StopEmitting);
        }
        else if (progress >= 1.5f)
        {
            Destroy(this.gameObject);
        }
    }
}