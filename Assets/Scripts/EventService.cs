using System;
using Enums;
using System.Collections.Generic;
using Interfaces;
using UnityEngine;
using UnityEngine.Events;

public class EventService : MonoBehaviour, IBootstrappable
{
    private static EventService _instance;

    public BootPriority BootPriority => BootPriority.Core;

    private static Dictionary<EventName, List<UnityAction>> _listeners;
    private static Dictionary<EventName, Dictionary<Type, List<Delegate>>> _listenersOneParam;
    
    public void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public void ManualStart()
    {
        _listeners = new Dictionary<EventName, List<UnityAction>>();
        _listenersOneParam = new Dictionary<EventName, Dictionary<Type, List<Delegate>>>();
    }

    public static void AddListener(EventName eventName, UnityAction listener)
    {
        if (_listeners.ContainsKey(eventName))
        {
            _listeners[eventName].Add(listener);
        }
        else
        {
            _listeners.Add(eventName, new List<UnityAction> {listener});
        }
    }
    
    public static void AddListener<T>(EventName eventName, UnityAction<T> listener)
    {
        if (!_listenersOneParam.ContainsKey(eventName))
        {
            _listenersOneParam.Add(eventName, new Dictionary<Type, List<Delegate>>());
        }

        var parameterType = listener.Method.GetParameters()[0].ParameterType;

        if (!_listenersOneParam[eventName].ContainsKey(parameterType))
        {
            _listenersOneParam[eventName].Add(parameterType, new List<Delegate>());
        }
        
        _listenersOneParam[eventName][parameterType].Add(listener);
    }

    public static void RemoveListener(EventName eventName, UnityAction listener)
    {
        if (!_listeners.ContainsKey(eventName))
        {
            return;
        }

        _listeners[eventName].Remove(listener);
    }

    public static void RemoveListener<T>(EventName eventName, UnityAction<T> listener)
    {
        var parameterType = listener.Method.GetParameters()[0].ParameterType;
        if (!_listenersOneParam.ContainsKey(eventName) || !_listenersOneParam[eventName].ContainsKey(parameterType))
        {
            return;
        }

        _listenersOneParam[eventName].Remove(parameterType);
    }

    public static void Invoke(EventName eventName)
    {
        if (!_listeners.ContainsKey(eventName))
        {
            return;
        }

        foreach (var listener in _listeners[eventName])
        {
            listener.Invoke();
        }
    }

    public static void Invoke<T>(EventName eventName, T data)
    {
        var parameterType = typeof(T);
        
        if (!_listenersOneParam.ContainsKey(eventName) || !_listenersOneParam[eventName].ContainsKey(parameterType))
        {
            return;
        }

        foreach (var @delegate in _listenersOneParam[eventName][parameterType])
        {
            var listener = @delegate as UnityAction<T>;
            listener?.Invoke(data);
        }
    }
}