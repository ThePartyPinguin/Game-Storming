using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseFactory<TInitGameObject, TFactory> : MonoSingleton<TFactory> where TInitGameObject : FactoryInitGameObject<TInitGameObject> where TFactory : BaseFactory<TInitGameObject, TFactory>
{
    private Dictionary<Type, Queue<TInitGameObject>> _inActiveGameObjectsCollection;
    private Dictionary<Type, Dictionary<Guid, TInitGameObject>> _createdGameObjectsCollection;

    void Awake()
    {
        _inActiveGameObjectsCollection = new Dictionary<Type, Queue<TInitGameObject>>();
        _createdGameObjectsCollection = new Dictionary<Type, Dictionary<Guid, TInitGameObject>>();
    }

    private void DisableObject(TInitGameObject o)
    {
        o.gameObject.SetActive(false);
        _inActiveGameObjectsCollection[o.GetType()].Enqueue(o);
    }

    public abstract void OnProducedNew(ref TInitGameObject newObject);

    public virtual TInitGameObject Produce(TInitGameObject prototype, params object[] initArgs)
    {
        return ProtectedProduce(prototype, initArgs);
    }

    public virtual TInitGameObject Produce(TInitGameObject prototype, Transform parent, params object[] initArgs)
    {
        return ProtectedProduce(prototype, parent, initArgs);
    }

    private TInitGameObject ProtectedProduce(TInitGameObject prototype, params object[] initArgs)
    {
        return CreateNewFromPrototype(prototype, null, initArgs);
    }

    private TInitGameObject ProtectedProduce(TInitGameObject prototype, Transform parent, params object[] initArgs)
    {
        return CreateNewFromPrototype(prototype, parent, initArgs);
    }

    private TInitGameObject CreateNewFromPrototype(TInitGameObject prototype, Transform parent, params object[] initArgs)
    {
        Type prototypeType = prototype.GetType();

        TInitGameObject o = null;

        if (!_inActiveGameObjectsCollection.ContainsKey(prototypeType))
            _inActiveGameObjectsCollection.Add(prototypeType, new Queue<TInitGameObject>());

        if (_inActiveGameObjectsCollection[prototypeType].Count > 0)
        {
            o = _inActiveGameObjectsCollection[prototypeType].Dequeue();
        }
        else
        {
            o = Instantiate(prototype);
            o.transform.name = prototypeType + "-" + _inActiveGameObjectsCollection[prototypeType].Count;
            o.transform.SetParent(parent);

            Guid internalFactoryId = Guid.NewGuid();

            o.Init(internalFactoryId, () =>
            {
                DisableObject(o);
            }, initArgs);

            if (!_createdGameObjectsCollection.ContainsKey(prototypeType))
                _createdGameObjectsCollection.Add(prototypeType, new Dictionary<Guid, TInitGameObject>());

            _createdGameObjectsCollection[prototypeType].Add(internalFactoryId, o);

            OnProducedNew(ref o);
        }

        return o;
    }
}
