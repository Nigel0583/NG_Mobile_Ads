using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    [SerializeField] private bool dontDestroy = false;

    private static T _mInstance;

    public static T Instance
    {
        get
        {
            if (_mInstance != null) return _mInstance;
            _mInstance = GameObject.FindObjectOfType<T>();
            if (_mInstance != null) return _mInstance;
            var singleton = new GameObject(typeof(T).Name);
            _mInstance = singleton.AddComponent<T>();
            return _mInstance;
        }
    }

    public virtual void Awake()
    {
        if (_mInstance == null)
        {
            _mInstance = this as T;

            if (!dontDestroy) return;
            transform.parent = null;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}