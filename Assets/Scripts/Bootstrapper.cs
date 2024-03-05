using System.Collections.Generic;
using System.Linq;
using Enums;
using Interfaces;
using UnityEngine;

public class Bootstrapper : MonoBehaviour
{
    private static SortedList<BootPriority, List<IBootstrappable>> _bootstrappables;

    public void Start()
    {
        _bootstrappables = new SortedList<BootPriority, List<IBootstrappable>>();
        
        var sceneBootstrappables = new List<IBootstrappable>(); 
        sceneBootstrappables.AddRange(FindObjectsOfType<MonoBehaviour>().OfType<IBootstrappable>());

        foreach (var bootstrappable in sceneBootstrappables)
        {
            var priority = bootstrappable.BootPriority;
            if (!_bootstrappables.ContainsKey(priority))
            {
                _bootstrappables.Add(priority, new List<IBootstrappable>() {bootstrappable});
            }
            else
            {
                _bootstrappables[priority].Add(bootstrappable);
            }
        }
        
        foreach (var keyValuePair in _bootstrappables)
        {
            var bootList = keyValuePair.Value;
            foreach (var bootstrappable in bootList)
            {
                bootstrappable.ManualStart();
            }
        }
    }
}