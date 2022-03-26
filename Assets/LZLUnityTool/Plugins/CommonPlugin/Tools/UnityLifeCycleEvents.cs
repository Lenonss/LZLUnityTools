using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class UnityLifeCycleEvents : MonoBehaviour
{
    

    [SerializeField]
    private List<UnityLifeCycleType> showedEvents = new List<UnityLifeCycleType>();

    [BoxGroup("Awake")]
    public UnityEvent OnAwakeEvent;

    [BoxGroup("OnEnable")]
    public UnityEvent OnEnableEvent;

    [BoxGroup("OnStart")]
    public UnityEvent OnStartEvent;
    
    [BoxGroup("Update")]
    public UnityEvent OnUpdatedEvent;
    
    [BoxGroup("FixedUpdate")]
    public UnityEvent OnFixedUpdateEvent;
    
    [BoxGroup("LateUpdate")]
    public UnityEvent OnLateUpdateEvent;

    [BoxGroup("OnDisable")]
    public UnityEvent OnDisableEvent;
    
    [BoxGroup("OnDestroy")]
    public UnityEvent OnDestroyEvent;
    
    [BoxGroup("OnApplicationQuit")]
    public UnityEvent OnApplicationQuitEvent;
    private void Awake()
    {
        OnAwakeEvent?.Invoke();
    }

    private void OnEnable()
    {
        OnEnableEvent?.Invoke();
    }

    private void Start()
    {
        OnStartEvent?.Invoke();
    }

    private void Update()
    {
        OnUpdatedEvent?.Invoke();
    }

    private void FixedUpdate()
    {
        OnFixedUpdateEvent?.Invoke();
    }

    private void LateUpdate()
    {
        OnLateUpdateEvent?.Invoke();
    }

    private void OnDisable()
    {
        OnDisableEvent?.Invoke();
    }

    private void OnDestroy()
    {
        OnDestroyEvent?.Invoke();
    }

    private void OnApplicationQuit()
    {
        OnApplicationQuitEvent?.Invoke();
    }
}

public enum UnityLifeCycleType
{
    Awake = 0,
    OnEnable= 1,
    Start = 2,
    Update = 3,
    FixedUpdate = 4,
    LateUpdate = 5,
    OnDisable = 6,
    OnDestroy = 7,
    OnApplicationQuit = 8
}
