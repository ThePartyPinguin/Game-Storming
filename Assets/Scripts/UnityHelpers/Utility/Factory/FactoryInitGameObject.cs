using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FactoryInitGameObject<TInitGameObject> : MonoBehaviour where TInitGameObject : FactoryInitGameObject<TInitGameObject>
{
    public Guid InternalFactoryObjectId { get; private set; }

    private Action _onDisable;

    public void Destroy()
    {
        _onDisable?.Invoke();
        OnDisabled();
    }

    protected abstract void OnDisabled();

    internal void Init(Guid internalFactoryObjectId, Action onDisable, params object[] args)
    {
        _onDisable = onDisable;
        InternalFactoryObjectId = internalFactoryObjectId;

        Init(args);
    }

    protected abstract void Init(params object[] args);


}
