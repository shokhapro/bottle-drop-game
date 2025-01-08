using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-100)]
public class GlobalEventSequence : MonoBehaviour
{
    [Header("Steps")]
    [SerializeField] private string[] globalKeys;
    [SerializeField] private GlobalEvent localHandler;

    private int _lastStep = -1;

    public void Step(int newStep)
    {
        if (newStep != _lastStep + 1) return;

        var key = globalKeys[newStep];

        if (localHandler != null)
            localHandler.Invoke(key);
        else
            GlobalEvent.InvokeGlobal(key);

        _lastStep = newStep;
    }

    public void Reset()
    {
        _lastStep = -1;
    }
}
