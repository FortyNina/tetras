using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool<T> where T : MonoBehaviour
{
    private Queue<T> _queue = new Queue<T>();
    private List<T> _all = new List<T>();
    private HashSet<T> _inPool = new HashSet<T>();
    private GameObject _template;
    private Transform _parent;

    public void Initialize(GameObject template, Transform parent = null)
    {
        _template = template;
        _parent = parent;
    }

    public T Get()
    {
        if (_template == null)
            throw new System.Exception("Pool used before Initialize().");

        T obj;

        if (_queue.Count == 0)
        {
            obj = Create();
            obj.gameObject.SetActive(true);
            return obj;
        }

        obj = _queue.Dequeue();
        obj.gameObject.SetActive(true);
        _inPool.Remove(obj);
        return obj;
    }

    private T Create()
    {
        T newObj = Object.Instantiate(_template, _parent).GetComponent<T>();
        _all.Add(newObj);
        return newObj;
    }

    public void Return(T obj)
    {
        if (_inPool.Contains(obj)) return;
        obj.gameObject.SetActive(false);
        _queue.Enqueue(obj);
        _inPool.Add(obj);

    }

    public void Clear()
    {
        foreach (T obj in _all)
        {
            Object.Destroy(obj.gameObject);
        }
        _all.Clear();
        _queue.Clear();
        _inPool.Clear();
    }
}