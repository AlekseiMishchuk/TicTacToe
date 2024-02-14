using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    private static EventManager Instance { get; set; }

    private static Dictionary<Event, List<UnityAction>> _listeners;
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            _listeners = new Dictionary<Event, List<UnityAction>>();
        }
    }

    public static void AddListener(Event invoker, UnityAction listener)
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

    public static void RemoveListener(Event invoker, UnityAction listener)
    {
        if (!_listeners.ContainsKey(invoker))
        {
            return;
        }

        _listeners[invoker].Remove(listener);
    }

    public static void Invoke(Event invoker)
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
}
public enum Event
{
    MoveMade
}