﻿using System;
using System.Collections.Generic;
using System.Reflection;
using Aya.Data.Persistent;
using Aya.Extension;
using UnityEngine;

public abstract class ConfigData
{
    public int ID;
}

public abstract class Config
{
    public virtual void Init()
    {
         
    }
}

public class Config<T> : Config where T : ConfigData
{
    public List<T> DataList;
    public Dictionary<int, T> DataDic;
    public int Count => DataList.Count;

    public override void Init()
    {
        DataDic = DataList.ToDictionary(d => d.ID);
    }

    public T this[int id] => DataDic[id];

    public T GetData(int index)
    {
        if (index < 0) return DataList.First();
        if (index >= Count) return DataList.Last();
        return DataList[index];
    }

    public T GetData(Predicate<T> predicate)
    {
        foreach (var data in DataList)
        {
            if (predicate(data)) return data;
        }

        return default;
    }

    public List<T> GetDatas(Predicate<T> predicate = null)
    {
        if (predicate == null) return DataList;
        var result = new List<T>();
        foreach (var data in DataList)
        {
            if (predicate(data)) result.Add(data);
        }

        return result;
    }


    public sInt Level = new sInt(typeof(T).Name, 1);
    public bool IsMaxLevel => Level >= Count;
    public bool CanUpgrade => Level < Count;

    public T Current
    {
        get
        {
            var index = Level.Value - 1;
            var data = DataList[index];
            return data;
        }
    }

    public bool Upgrade()
    {
        if (IsMaxLevel) return false;
        Level.Value += 1;
        return true;
    }
}

public class ConfigManager : GameEntity<ConfigManager>
{
    public static Dictionary<Type, Config> ConfigDic = new Dictionary<Type, Config>();

    public T GetData<T>(int index) where T : ConfigData
    {
        return GetConfig<T>().GetData(index);
    }

    public T GetData<T>(Predicate<T> predicate) where T : ConfigData
    {
        return GetConfig<T>().GetData(predicate);
    }

    public List<T> GetDatas<T>(Predicate<T> predicate = null) where T : ConfigData
    {
        return GetConfig<T>().GetDatas(predicate);
    }

    public Config<T> GetConfig<T>() where T : ConfigData
    {
        if (!ConfigDic.TryGetValue(typeof(T), out var config))
        {
            config = LoadConfig<T>();
            ConfigDic.Add(typeof(T), config);
        }

        return config as Config<T>;
    }

    public Config<T> LoadConfig<T>() where T : ConfigData
    {
        var path = "Config/" + typeof(T).Name;
        var text = Resources.Load<TextAsset>(path).text;
        var lines = text.Split('\n');
        var dataList = new List<T>();
        var fieldDic = new Dictionary<string, FieldInfo>();
        for (var i = 0; i < lines.Length; i++)
        {
            var line = lines[i];
            var str = line.Trim();
            if (string.IsNullOrEmpty(str)) continue;

            var strArray = str.Split(',');
            if (i == 0)
            {
                foreach (var fieldName in strArray)
                {
                    var fieldInfo = typeof(T).GetField(fieldName);
                    fieldDic.Add(fieldName, fieldInfo);
                }
            }
            else
            {
                var data = Activator.CreateInstance<T>();
                var index = 0;
                foreach (var filedInfo in fieldDic.Values)
                {
                    var value = strArray[index];
                    if (filedInfo.FieldType == typeof(int))
                    {
                        filedInfo.SetValue(data, value.AsInt());
                    }
                    else if (filedInfo.FieldType == typeof(float))
                    {
                        filedInfo.SetValue(data, value.AsFloat());
                    }
                    else if (filedInfo.FieldType == typeof(string))
                    {
                        filedInfo.SetValue(data, value.AsString());
                    }

                    index++;
                }

                dataList.Add(data);
            }
        }

        var config = new Config<T> {DataList = dataList};
        config.Init();
        return config;
    }
}
