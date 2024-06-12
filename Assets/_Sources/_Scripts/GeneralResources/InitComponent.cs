
using System;
using System.Collections.Generic;
using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

public class InitComponent : MonoBehaviour
{
#if ODIN_INSPECTOR
    [Button]
    public List<IInit> GetComp()
    {
        List<IInit> list = new List<IInit>();

        foreach (var i in GetComponentsInChildren<IInit>(true))
        {
            list.Add(i);
        }

        return list;
    }
#endif
    public bool initOnAwake;
    public bool initOnStart = true;

    private void Awake()
    {
        if (initOnAwake) Init();
    }
    
    private void Start()
    {
        if (initOnStart) Init();
    }

    private void Init()
    {
        foreach (var comp in GetComponentsInChildren<IInit>(true))
        {
            comp.Init();
        }
    }
}

public interface IInit
{
    void Init();
}