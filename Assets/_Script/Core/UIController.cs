using System;
using System.Collections;
using System.Collections.Generic;
using Aya.Singleton;
using UnityEngine;

public class UIController : GameEntity<UIController>
{
    public Dictionary<Type, UIBase> UIDic= new Dictionary<Type, UIBase>();
    public UIBase Current { get; set; }

    protected override void Awake()
    {
        base.Awake();

        var uis = transform.GetComponentsInChildren<UIBase>(true);
        foreach (var ui in uis)
        {
            UIDic.Add(ui.GetType(), ui);
        }

        HideAll();
    }

    public void Show<T>() where T : UIBase
    {
        var type = typeof(T);
        Current?.Hide();

        if (UIDic.TryGetValue(type, out var ui))
        {
            ui.Show();
            Current = ui;
        }
    }

    public void HideAll()
    {
        foreach (var kv in UIDic)
        {
            var ui = kv.Value;
            ui.Hide();
        }
    }
}
