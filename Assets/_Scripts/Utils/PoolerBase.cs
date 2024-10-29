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

    protected void InitPool(T prefab, GameObject holder = null, int initial = 100, int max = 1000, bool collectionChecks = false) {
        _prefab = prefab;
        _holder = holder;
        Pool = new ObjectPool<T>(
            CreateSetup,
            GetSetup,
            ReleaseSetup,
            DestroySetup,
            collectionChecks);
    }

    #region Overrides
    protected virtual T CreateSetup()
    {
        T obj = Instantiate(_prefab);
        if (_holder != null) obj.transform.SetParent(_holder.transform);
        obj.gameObject.SetActive(false);
        Debug.Log($"Created: {obj.name}");
        return obj;
    }
    protected virtual void GetSetup(T obj)
    {
        if (obj == null)
        {
            Debug.LogWarning("Trying to activate a null object in the pool.");
            return; // Or handle this case as you see fit
        }
        obj.gameObject.SetActive(true);
    }
    protected virtual void ReleaseSetup(T obj)
    {

        if (obj != null)
        {
            Debug.Log($"Releasing: {obj.name}");
            obj.gameObject.SetActive(false);
            // Optional: You can add any additional cleanup logic here
        }
    }


    protected virtual void DestroySetup(T obj)
    {
        Debug.Log($"Destroying: {obj.name}");
        Destroy(obj);
    }
    #endregion

    #region Getters
    public T Get()
    {
        T obj = Pool.Get();

        if (obj == null)
        {
            obj = CreateSetup();
        }

        return obj;

    }
    public void Release(T obj) => Pool.Release(obj);
    #endregion
}
