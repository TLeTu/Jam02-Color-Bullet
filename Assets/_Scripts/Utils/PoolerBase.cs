using System;
using System.Collections.Generic;
using UnityEngine;
public abstract class PoolerBase<T> : MonoBehaviour where T : MonoBehaviour 
{
    [Header("Pooler Settings")]
    [SerializeField] protected T _prefab;
    [SerializeField] protected GameObject _holder;
    [Space(50)]

    protected List<T> _pool;

    private int _count = 0;

    private List<T> Pool {
        get {
            if (_pool == null) throw new InvalidOperationException("You need to call InitPool before using it.");
            return _pool;
        }
        set => _pool = value;
    }

    protected virtual void Awake()
    {
        if (_prefab == null) throw new InvalidOperationException("Prefab is not set.");
        if (_holder == null) throw new InvalidOperationException("Holder is not set.");

        InitPool(_prefab, _holder);
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

        obj.name = _prefab.name + " - " + _count;
        _count++;

        GetSetup(obj);

        Pool.Add(obj);
    }

    protected void Return(T obj)
    {
        obj.gameObject.SetActive(false);
        Pool.Add(obj);
    }
}
