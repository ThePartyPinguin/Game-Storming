using UnityEngine;

public abstract class InitMonoBehaviour : MonoBehaviour
{
    public abstract void Init();
}
public abstract class InitMonoBehaviour<T1> : MonoBehaviour
{
    public abstract void Init(T1 param);
}

public abstract class InitMonoBehaviour<T1, T2> : MonoBehaviour
{
    public abstract void Init(T1 param, T2 param2);
}

public abstract class InitMonoBehaviour<T1, T2, T3> : MonoBehaviour
{
    public abstract void Init(T1 param, T2 param2, T3 param3);
}

public abstract class InitMonoBehaviour<T1, T2, T3, T4> : MonoBehaviour
{
    public abstract void Init(T1 param, T2 param2, T3 param3, T4 param4);

}
