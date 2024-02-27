using System;
using Enums;
using System.Collections.Generic;
using System.Linq;
using Interfaces;
using UnityEngine;
using UnityEngine.Events;

public class EventService : MonoBehaviour, IOnAwakeCompleted
{
    private static EventService Instance { get; set; }

    private static List<IOnAwakeCompleted> _awakeCompletedListeners = new ();
    private static Dictionary<EventName, List<UnityAction>> _listeners;
    private static Dictionary<EventName, Dictionary<Type, List<Delegate>>> _listenersOneParam;
    
    private void Awake()
    {
        _awakeCompletedListeners.AddRange(FindObjectsOfType<MonoBehaviour>().OfType<IOnAwakeCompleted>());
        if (Instance == null)
        {
            Instance = this;
            _listeners = new Dictionary<EventName, List<UnityAction>>();
            _listenersOneParam = new Dictionary<EventName, Dictionary<Type, List<Delegate>>>(); 
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }

    public static void Reload()
    {
        _awakeCompletedListeners = new List<IOnAwakeCompleted>();
        _awakeCompletedListeners.AddRange(FindObjectsOfType<MonoBehaviour>().OfType<IOnAwakeCompleted>());
        _listeners = new Dictionary<EventName, List<UnityAction>>();
        _listenersOneParam = new Dictionary<EventName, Dictionary<Type, List<Delegate>>>();
    }

    public static void AddListener(EventName invoker, UnityAction listener)
    {
        if (_listeners.ContainsKey(invoker))
        {
            _listeners[invoker].Add(listener);
        }
        else
        {
            _listeners.Add(invoker, new List<UnityAction> {listener});
        }
    }
    
    public static void AddListener<T>(EventName invoker, UnityAction<T> listener)
    {
        if (!_listenersOneParam.ContainsKey(invoker))
        {
            _listenersOneParam.Add(invoker, new Dictionary<Type, List<Delegate>>());
        }

        var parameterType = listener.Method.GetParameters()[0].ParameterType;

        if (!_listenersOneParam[invoker].ContainsKey(parameterType))
        {
            _listenersOneParam[invoker].Add(parameterType, new List<Delegate>());
        }
        
        _listenersOneParam[invoker][parameterType].Add(listener);
    }

    public static void RemoveListener(EventName invoker, UnityAction listener)
    {
        if (!_listeners.ContainsKey(invoker))
        {
            return;
        }

        _listeners[invoker].Remove(listener);
    }

    public static void RemoveListener<T>(EventName invoker, UnityAction<T> listener)
    {
        var parameterType = listener.Method.GetParameters()[0].ParameterType;
        if (!_listenersOneParam.ContainsKey(invoker) || !_listenersOneParam[invoker].ContainsKey(parameterType))
        {
            return;
        }

        _listenersOneParam[invoker].Remove(parameterType);
    }

    public static void Invoke(EventName invoker)
    {
        if (!_listeners.ContainsKey(invoker))
        {
            return;
        }

        foreach (var listener in _listeners[invoker])
        {
            listener.Invoke();
        }
    }

    public static void Invoke<T>(EventName invoker, T data)
    {
        var parameterType = typeof(T);
        
        if (!_listenersOneParam.ContainsKey(invoker) || !_listenersOneParam[invoker].ContainsKey(parameterType))
        {
            return;
        }

        foreach (var @delegate in _listenersOneParam[invoker][parameterType])
        {
            var listener = @delegate as UnityAction<T>;
            listener?.Invoke(data);
        }
    }

    public static void AwakeCompleted(IOnAwakeCompleted invoker)
    {
        _awakeCompletedListeners.Remove(invoker);
        if (_awakeCompletedListeners.Count == 0)
        {
            GameManager.LateAwake();
        }
    }

    public void OnAwakeCompleted()
    {
        AwakeCompleted(this);
    }
}