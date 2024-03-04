using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Enums;
using Interfaces;
using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    private static SortedList<BootPriority, List<IManualInitialization>> _initializables;

    public void Start()
    {
        _initializables = new SortedList<BootPriority, List<IManualInitialization>>();
        
        var list = new List<IManualInitialization>(); 
        list.AddRange(FindObjectsOfType<MonoBehaviour>().OfType<IManualInitialization>());

        foreach (var init in list)
        {
            var priority = init.BootPriority;
            if (!_initializables.ContainsKey(priority))
            {
                _initializables.Add(priority, new List<IManualInitialization>() {init});
            }
            else
            {
                _initializables[priority].Add(init);
            }
        }
        
        foreach (var keyValuePair in _initializables)
        {
            var initObjects = keyValuePair.Value;
            foreach (var @object in initObjects)
            {
                @object.ManualInit();
            }
        }
    }
}