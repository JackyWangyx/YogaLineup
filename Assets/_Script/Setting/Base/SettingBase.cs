using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SettingBase<T> : ScriptableObject where T : SettingBase<T>
{
    #region Ins / Load

    public static T Ins
    {
        get
        {
            if (_instance == null) _instance = Load<T>();
            return _instance;
        }
    }

    private static T _instance;

    public static TSetting Load<TSetting>() where TSetting : SettingBase<TSetting>
    {
        var setting = Resources.Load<TSetting>("Setting/" + typeof(TSetting).Name);
        setting.Init();
        return setting;
    } 

    #endregion

    public virtual void Init()
    {

    }
}
