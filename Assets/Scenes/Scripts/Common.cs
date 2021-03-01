using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class Common
{
    public static T SetComponent<T>(GameObject go) where T : Component
    {
        var component = go.GetComponent<T>();
        if (component == null)
            return go.AddComponent<T>();
        else
            return component;
    }

    public static List<T> GetComponets<T>(Transform parent)
    {
        List<T> children = new List<T>();
        children = GetChildren(parent, children);
        for (int n = 0; n < children.Count; n++)
        {
            if (children[n] == null)
            {
                children.RemoveAt(n);
            }
        }
        return children;
    }
    /// <summary>
    /// Рекурсией обходим все кости персонажа ищем в них T, в скелете любого типа.
    /// </summary>
    private static List<T> GetChildren<T>(Transform parent, List<T> children)
    {
        //if (children.Count > 200) throw new Exception("Recursion Error!");
        for (int n = 0; n < parent.childCount; n++)
        {
            Transform tr = parent.GetChild(n);
            T component = tr.GetComponent<T>();

            GetChildren<T>(tr, children);
            if (component != null && !children.Contains(component))
            {
                Debug.Log(tr.name);
                children.Add(component);
            }
        }
            /*if (component != null && !children.Contains(component))
            {
                children.Add(component);
            }
            if (tr.childCount > 0)
            {
                var array = GetChildren<T>(tr, children);
                for (int m = 0; m < array.Count; m++)
                {
                    if (array[m] != null )
                        children.Add(array[m]);
                }
            }*/
        return children;
    }
}