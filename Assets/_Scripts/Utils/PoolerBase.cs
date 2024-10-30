using System;
using System.Collections.Generic;
using UnityEngine;
public abstract class PoolerBase<T> : MonoBehaviour where T : MonoBehaviour 
{
    private T _prefab;
    private List<T> _pool;
    private GameObject _holder;

    private List<T> Pool {
        get {
            if (_pool == null) throw new InvalidOperationException("You need to call InitPool before using it.");
            return _pool;
        }
        set => _pool = value;
    }

    protected void InitPool(T prefab, GameObject holder = null) {
        _prefab = prefab;
        _holder = holder;

        Pool = new List<T>();
    }

    protected T Get()
    {
        if (Pool.Count == 0)
        {
            CreateNew();
        }

        var obj = Pool[0];
        Pool.RemoveAt(0);
        obj.gameObject.SetActive(true);

        Initialize(obj);

        return obj;
    }

    protected abstract void Initialize(T obj);

    protected abstract void GetSetup(T obj);

    private void CreateNew()
    {
        var obj = Instantiate(_prefab, _holder.transform);
        obj.gameObject.SetActive(false);

        GetSetup(obj);

        Pool.Add(obj);
    }

    protected void Return(T obj)
    {
        obj.gameObject.SetActive(false);
        Pool.Add(obj);
    }
}
