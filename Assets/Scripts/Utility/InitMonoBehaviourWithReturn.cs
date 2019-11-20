using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InitMonoBehaviourWithReturn<T1> : MonoBehaviour
{
    public abstract T1 Init();
}

public abstract class InitMonoBehaviourWithReturn<T1, T2> : MonoBehaviour
{
    public abstract T1 Init(T2 param);
}

public abstract class InitMonoBehaviourWithReturn<T1, T2, T3> : MonoBehaviour
{
    public abstract T1 Init(T2 param1, T3 param2);
}

public abstract class InitMonoBehaviourWithReturn<T1, T2, T3, T4> : MonoBehaviour
{
    public abstract T1 Init(T2 param1, T3 param2, T4 param3);
}
