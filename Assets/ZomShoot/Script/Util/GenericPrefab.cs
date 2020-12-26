using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

public class PrefabPath : Attribute
{
    public string Path { get; set; }
    public PrefabPath(string path)
    {
        Path = path;
    }
}

public class GenericPrefab
{
    public static T Instantiate<T>()
    {
        var prefabPathAttr = typeof(T).GetCustomAttribute<PrefabPath>();
        var prefab = Resources.Load(prefabPathAttr.Path) as GameObject;
        var go = GameObject.Instantiate(prefab, Vector3.zero, Quaternion.identity);
        return go.GetComponent<T>();
    }

    public static T Instantiate<T>(Transform parent)
    {
        var prefabPathAttr = typeof(T).GetCustomAttribute<PrefabPath>();
        var prefab = Resources.Load(prefabPathAttr.Path) as GameObject;
        var go = GameObject.Instantiate(prefab, parent.position, parent.rotation, parent);
        return go.GetComponent<T>();
    }

    public static T InstantiatePathFormat<T>(string a0)
    {
        var prefabPathAttr = typeof(T).GetCustomAttribute<PrefabPath>();
        var path = string.Format(prefabPathAttr.Path, a0);
        var prefab = Resources.Load(path) as GameObject;
        var go = GameObject.Instantiate(prefab, Vector3.zero, Quaternion.identity);
        return go.GetComponent<T>();
    }
}
