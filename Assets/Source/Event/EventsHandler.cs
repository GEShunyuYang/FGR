using System;
using System.Collections.Generic;
using UnityEngine;

public class EventsHandler : MonoBehaviour
{
    private static EventsHandler _instance;
    public static bool isQuit = false;
    public static EventsHandler Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<EventsHandler>();
                if (_instance == null)
                {
                    if (isQuit) return _instance;
                    GameObject obj = new GameObject("EventsHandler");
                    _instance = obj.AddComponent<EventsHandler>();
                    DontDestroyOnLoad(obj);
                }
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this) {
            Destroy(gameObject);
            return;
        }
    }

    private void OnDestroy()
    {
        isQuit = true;
        ClearAllEvents();
    }

    private Dictionary<string, Action> eventDictionary = new();
    private Dictionary<string, Action<object>> paramEventDictionary = new();

    public static void RegisterEvent(string name, Action action)
    {
        if (!Instance.eventDictionary.ContainsKey(name))
        {
            Instance.eventDictionary.Add(name, action);
        }
    }
    public static void RegisterEvent<T>(string name, Action<T> action)
    {
        string key = $"{name}_{typeof(T).Name}";
        if (!Instance.paramEventDictionary.ContainsKey(key))
        {
            Instance.paramEventDictionary.Add(key, (obj) => action((T)obj));
        }
    }

    public static void UnregisterEvent(string name, Action action) { 
        if(Instance && Instance.eventDictionary.ContainsKey(name))
        {
            Instance.eventDictionary[name] -= action;
        }
    }

    public static void UnregisterEvent<T>(string name, Action<T> action)
    {
        string key = $"{name}_{typeof(T).Name}";
        if (Instance && Instance.paramEventDictionary.ContainsKey(key))
        {
            Action<object> target = (obj) => action((T)obj);
            Instance.paramEventDictionary[key] -= target;
        }
    }

    public static void TriggerEvent(string name) {
        if (Instance.eventDictionary.TryGetValue(name, out Action action)) {
            action?.Invoke();
        } else {
            Debug.LogWarning($"event {name} not registered");
        }
    }

    public static void TriggerEvent<T>(string name, T eventData)
    {
        string key = $"{name}_{typeof(T).Name}";
        if (Instance.paramEventDictionary.TryGetValue(key, out Action<object> action))
        {
            action?.Invoke(eventData);
        }
        else
        {
            Debug.LogWarning($"event {key} not registered");
        }
    }

    private void ClearAllEvents()
    {
        eventDictionary.Clear();
        paramEventDictionary.Clear();
    }
}