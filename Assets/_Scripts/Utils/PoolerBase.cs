using System;
using UnityEngine;
using UnityEngine.Pool;

/// <summary>
/// A simple base class to simplify object pooling in Unity 2021.
/// Derive from this class, call InitPool and you can Get and Release to your hearts content.
/// </summary>
public abstract class PoolerBase<T> : MonoBehaviour where T : MonoBehaviour 
{
    private T _prefab;
    private ObjectPool<T> _pool;
    private GameObject _holder;

    private ObjectPool<T> Pool {
        get {
            if (_pool == null) throw new InvalidOperationException("You need to call InitPool before using it.");
            return _pool;
        }
        set => _pool = value;
    }

    protected void InitPool(T prefab, GameObject holder = null, int initial = 10, int max = 20, bool collectionChecks = false) {
        _prefab = prefab;
        _holder = holder;
        Pool = new ObjectPool<T>(
            CreateSetup,
            GetSetup,
            ReleaseSetup,
            DestroySetup,
            collectionChecks,
            initial,
            max);
    }

    #region Overrides
    protected virtual T CreateSetup()
    {
        T obj = Instantiate(_prefab);
        if (_holder != null) obj.transform.SetParent(_holder.transform);
        return obj;
    }
    protected virtual void GetSetup(T obj) => obj.gameObject.SetActive(true);
    protected virtual void ReleaseSetup(T obj) => obj.gameObject.SetActive(false);
    protected virtual void DestroySetup(T obj) => Destroy(obj);
    #endregion

    #region Getters
    public T Get() => Pool.Get();
    public void Release(T obj) => Pool.Release(obj);
    #endregion
}
