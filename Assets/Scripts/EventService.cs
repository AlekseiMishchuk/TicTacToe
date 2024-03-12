using System;
using Enums;
using System.Collections.Generic;
using UnityEngine.Events;

public class EventService
{
    private static readonly Dictionary<EventName, List<UnityAction>> _listeners = new ();
    private static readonly Dictionary<EventName, Dictionary<Type, List<Delegate>>> _listenersOneParam = new ();

    public EventService()
    {
        if (_listeners.Count > 0)
        {
            _listeners.Clear();
        }
        if (_listenersOneParam.Count > 0)
        {
            _listenersOneParam.Clear();
        }
    }
    public void AddListener(EventName eventName, UnityAction listener)
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
    
    public void AddListener<T>(EventName eventName, UnityAction<T> listener)
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

    public void RemoveListener(EventName eventName, UnityAction listener)
    {
        if (!_listeners.ContainsKey(eventName))
        {
            return;
        }

        _listeners[eventName].Remove(listener);
    }

    public void RemoveListener<T>(EventName eventName, UnityAction<T> listener)
    {
        var parameterType = listener.Method.GetParameters()[0].ParameterType;
        if (!_listenersOneParam.ContainsKey(eventName) || !_listenersOneParam[eventName].ContainsKey(parameterType))
        {
            return;
        }

        _listenersOneParam[eventName].Remove(parameterType);
    }

    public void Invoke(EventName eventName)
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

    public void Invoke<T>(EventName eventName, T data)
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