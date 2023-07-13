using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pool<T>
{
    private List<T> m_pool = new List<T>();
    private Func<T> CreateItem { get; }
    private Action<T> OnTakeFromPool { get;  }
    private Action<T> OnReturnToPool { get;  }
    private Action<T> OnDestroyPoolObject { get; }
    public int maxPoolSize { get; private set; }
    
    public Pool(Func<T> createItem, Action<T> onTakeFromPool, Action<T> onReturnToPool, Action<T> onDestroyPoolObject, int maxPoolSize)
    {
        CreateItem = createItem;
        OnTakeFromPool = onTakeFromPool;
        OnReturnToPool = onReturnToPool;
        OnDestroyPoolObject = onDestroyPoolObject;
        this.maxPoolSize = maxPoolSize;
    }

    public T Get()
    {
        return TakeFromPool();
    }

    public void Release(T item)
    {
        ReturnToPool(item);
    }

    private T TakeFromPool()
    {
        if (!m_pool.Any())
        {
            m_pool.Add(CreateItem());
        }
        T item = m_pool.Last();
        OnTakeFromPool(item);
        m_pool.Remove(item);
        return item;
    }

    private void ReturnToPool(T item)
    {
        if (m_pool.Count >= maxPoolSize)
        {
            DestroyPoolObject(item);
        }
        else
        {
            if (!m_pool.Contains(item))
            {
                OnReturnToPool(item);
                m_pool.Add(item);
            }
        }
        
    }
    
    private void DestroyPoolObject(T item)
    {
        OnDestroyPoolObject(item);
    }
    

}
