using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[DefaultExecutionOrder(-100)]
public class GlobalEvent : MonoBehaviour
{
    private static List<GlobalEvent> _all = new List<GlobalEvent>();

    [Serializable] public class KeyEvent
    {
        public string globalKey = "";
        public float delay = 0f;
        public UnityEvent action = new UnityEvent();
        public Coroutine delayedAction = null;
    }

    public bool active = true;
    [SerializeField] private KeyEvent[] events;

    private void Awake()
    {
        if (active) _all.Add(this);
    }

    private void Start()
    {
        Invoke("on-start");
    }

    private void OnEnable()
    {
        Invoke("on-enable");
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    public static void InvokeGlobal(string key)
    {
        foreach (var ge in _all.ToList()) ge.Invoke(key);

        foreach (var ga in _evs.ToList())
            if (ga.globalKey == key)
                ga.action.Invoke();
    }

    public void Invoke(string key)
    {
        var invoked = false;

        foreach (var e in events)
            if (e.globalKey != "" && e.globalKey == key)
            {
                if (e.delay > 0 && gameObject.activeInHierarchy)
                {
                    if (e.delayedAction != null) StopCoroutine(e.delayedAction);

                    e.delayedAction = StartCoroutine(Delaying(e.delay, e.action.Invoke));
                }
                else
                {
                    e.action.Invoke();
                }

                invoked = true;
            }

        //if (!invoked) Debug.Log("Calling event \"" + key + "\"");

        IEnumerator Delaying(float delay, UnityAction action)
        {
            if (delay < 0.01)
                yield return null;
            else
                yield return new WaitForSeconds(delay);

            action.Invoke();
        }
    }

    public void InvokeEventInChildren(string key)
    {
        var ges = transform.GetComponentsInChildren<GlobalEvent>();

        foreach (var ge in ges) ge.Invoke(key);
    }

    private void OnDestroy()
    {
        _all.Remove(this);
    }

    public KeyEvent[] GetEvents()
    {
        return events;
    }

    public void SetEvents(KeyEvent[] newEvents)
    {
        events = newEvents;
    }

    private static List<KeyAction> _evs = new List<KeyAction>();

    public class KeyAction
    {
        public string globalKey = "";
        public UnityAction action = null;
    }

    public static KeyAction AddVirtual(string key, UnityAction action)
    {
        var a = new KeyAction();
        a.globalKey = key;
        a.action = action;

        _evs.Add(a);

        return a;
    }

    public static void RemoveVirtual(KeyAction keyAction)
    {
        if (_evs.Contains(keyAction))
            _evs.Add(keyAction);
    }
}

public static class GlobalEventExtra
{
    public static void InvokeEvent(this MonoBehaviour t, string key)
    {
        var ge = t.GetComponent<GlobalEvent>();

        if (ge != null) ge.Invoke(key);
    }

    public static void InvokeEvent(this GameObject t, string key)
    {
        var ge = t.GetComponent<GlobalEvent>();

        if (ge != null) ge.Invoke(key);
    }

    public static void InvokeEventInChildren(this Transform t, string key)
    {
        var ges = t.GetComponentsInChildren<GlobalEvent>();

        foreach (var ge in ges) ge.Invoke(key);
    }
}
