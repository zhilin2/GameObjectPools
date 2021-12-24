
using System;
using UnityEngine;
using UnityEngine.Internal;
using Object = UnityEngine.Object;
public class MyObject : Object
{
    public static Action<Object, Action> DestroyAction = DefultDestroy;
    public delegate Object InstanObject(Object obj);
    public delegate GameObject InstanActionByTag(string s, InstanObject a);
    public delegate Object InstanAction(Object s, InstanObject a);
    public static InstanActionByTag InstantiateActionByTag = DefultInstantiateByTag;
    public static InstanAction InstantiateAction = DefultInstantiate;

    static void OnDestroy(Object obj, Action action)
    {
        DestroyAction(obj, action);
    }

    static void DefultDestroy(Object obj, Action action)
    {
        action();
    }

    static Object DefultInstantiate(Object obj, InstanObject action)
    {
        return action(obj);
    }

    static GameObject DefultInstantiateByTag(string tag, InstanObject action)
    {
        return action(Resources.Load(tag)) as GameObject;
    }

    new public static void Destroy(Object obj)
    {
        DestroyAction(obj, () =>
    {
        Object.Destroy(obj);
    });
    }

    new public static void Destroy(Object obj, [DefaultValue("0.0F")] float t)
    {
        DestroyAction(obj, () =>
        {
            Object.Destroy(obj, t);
        });
    }

    public static Object Instantiate(string tag)
    {
        return InstantiateActionByTag(tag, (original) =>
        {
            return Object.Instantiate(original);
        });
    }

    public static Object Instantiate(string tag, Transform parent)
    {
        return InstantiateActionByTag(tag, (original) =>
        {
            return Object.Instantiate(original, parent);
        });
    }

    public static Object Instantiate(string tag, Vector3 position, Quaternion rotation)
    {
        return InstantiateActionByTag(tag, (original) =>
        {
            return Object.Instantiate(original, position, rotation);
        });
    }

    public static Object Instantiate(string tag, Transform parent, bool worldPositionStays)
    {
        return InstantiateActionByTag(tag, (original) =>
        {
            return Object.Instantiate(original, parent, worldPositionStays);
        });
    }
    public static Object Instantiate(string tag, Vector3 position, Quaternion rotation, Transform parent)
    {
        return InstantiateActionByTag(tag, (original) =>
        {
            return Object.Instantiate(original, position, rotation, parent);
        });
    }

    new public static Object Instantiate(Object _original)
    {
        return InstantiateAction(_original, (original) =>
        {
            return Object.Instantiate(original);
        });
    }

    new public static Object Instantiate(Object _original, Transform parent)
    {
        return InstantiateAction(_original, (original) =>
        {
            return Object.Instantiate(original, parent);
        });
    }

    new public static Object Instantiate(Object _original, Vector3 position, Quaternion rotation)
    {
        return InstantiateAction(_original, (original) =>
        {
            return Object.Instantiate(original, position, rotation);
        });
    }

    new public static Object Instantiate(Object _original, Transform parent, bool worldPositionStays)
    {
        return InstantiateAction(_original, (original) =>
        {
            return Object.Instantiate(original, parent, worldPositionStays);
        });
    }

    new public static Object Instantiate(Object _original, Vector3 position, Quaternion rotation, Transform parent)
    {
        return InstantiateAction(_original, (original) =>
        {
            return Object.Instantiate(original, position, rotation, parent);
        });
    }
}