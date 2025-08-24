using System.Collections.Generic;
using UnityEngine;

public static class TransformExtensions
{
    /// <summary>
    /// 获取所有直接子节点
    /// </summary>
    public static List<Transform> GetAllDirectChildren(this Transform parent)
    {
        List<Transform> result = new List<Transform>();
        foreach (Transform child in parent)
        {
            result.Add(child);
        }
        return result;
    }
    
    /// <summary>
    /// 获取所有子节点，包括递归子节点
    /// </summary>
    public static List<Transform> GetAllChildren(this Transform parent)
    {
        List<Transform> result = new List<Transform>();
        foreach (Transform child in parent)
        {
            result.Add(child);
            result.AddRange(child.GetAllChildren()); // 递归
        }
        return result;
    }
    
    public static T GetComponentInTargetChild<T>(this Transform transform, string childName) where T : Component
    {
        foreach (Transform child in transform.GetAllChildren())
        {
            if (child.name == childName)
            {
                return child.GetComponent<T>();
            }
        }
        return null;
    }
}