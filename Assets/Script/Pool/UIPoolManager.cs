using UnityEngine;
using System;
using System.Collections.Generic;
public class UIPoolManager : MonoBehaviour
{
    private static UIPoolManager _pool;
    public static UIPoolManager pool
    {
        get { return _pool; }
        set
        {
            _pool = value;
            MyObject.InstantiateAction = InstantiateAction;
            MyObject.InstantiateActionByTag = InstantiateActionByTag;
            MyObject.DestroyAction = DestroyAction;

        }
    }

    void Start()
    {
        pool = this;
    }

    Dictionary<string, List<GameObject>> gameObjectPool = new Dictionary<string, List<GameObject>>();
    Dictionary<string, Action<GameObject>> onTakeOuts = new Dictionary<string, Action<GameObject>>();
    Dictionary<string, Action<GameObject>> onRecycles = new Dictionary<string, Action<GameObject>>();

    static GameObject InstantiateActionByTag(string tag, MyObject.InstanObject action)
    {
        List<GameObject> objs;
        GameObject obj;
        if (pool.gameObjectPool.TryGetValue(tag, out objs)&& objs.Count>0)
        {
            obj = objs[objs.Count - 1];
            RecycleComponent rc = obj.GetComponent<RecycleComponent>();
            rc.IsInPool = false;
            obj.SetActive(true);
            if (pool.onTakeOuts.ContainsKey(tag))
            {
                pool.onTakeOuts[tag](obj);
            }
            Action<GameObject> onTO = rc.onTakeOut;
            if (onTO != null)
                onTO(obj);
            objs.RemoveAt(objs.Count - 1);
            obj.transform.localScale = Vector3.one;
            return obj;
        }
        else
        {
            GameObject obj1 = Resources.Load(tag) as GameObject;
            if(obj1==null)
            {
                Debug.LogError("this path have\'t GameObject!!!   :" + tag);
                return null;
            }
            return action(obj1) as GameObject;
        }
    }

    static UnityEngine.Object InstantiateAction(UnityEngine.Object obj, MyObject.InstanObject action)
    {
        if (obj.GetType() == typeof(GameObject))
        {
            GameObject gObj = obj as GameObject;
            RecycleComponent rc = gObj.GetComponent<RecycleComponent>();
            if (rc != null)
            {
                string tag = rc.tag;
                List<GameObject> objs;
                if (pool.gameObjectPool.TryGetValue(tag, out objs) && objs.Count > 0)
                {
                    if (objs.Count > 0)
                    {
                        GameObject obj1 = objs[objs.Count - 1];
                        rc = obj1.GetComponent<RecycleComponent>();
                        rc.IsInPool = false;
                        obj1.SetActive(true);
                        if(pool.onTakeOuts.ContainsKey(tag))
                        {
                            pool.onTakeOuts[tag](obj1);
                        }
                        Action<GameObject> onTO = rc.onTakeOut;
                        if (onTO != null)
                            onTO(obj1);
                        objs.RemoveAt(objs.Count - 1);
                        obj1.transform.localScale = Vector3.one;
                        return obj1;
                    }
                }

            }
        }
        return action(obj);
    }

    static void DestroyAction(UnityEngine.Object tag, Action action)
    {
        if (tag && tag.GetType() == typeof(GameObject))
        {
            if ((tag as GameObject).transform.parent == _pool.transform)
            {
                Debug.LogError("你正在尝试删池子里的东西，会出问题的！！！     "+ (tag as GameObject).name);
                return;
            }
            RecycleComponent[] temp = (tag as GameObject).GetComponentsInChildren<RecycleComponent>();
            for (int i = 0; i < temp.Length; i++)
            {
                MoveToPool(temp[i]);
            }
            if ((tag as GameObject).GetComponent<RecycleComponent>() != null)
            {
                return;
            }
            (tag as GameObject).SetActive(false);
            action();
        }
        else
        {
            action();
        }
    }

    static void MoveToPool(RecycleComponent obj)
    {
        obj.IsInPool = true;
        if (!pool.gameObjectPool.ContainsKey(obj.tag))
        {
            pool.gameObjectPool.Add(obj.tag, new List<GameObject>());
        }
        if(pool.onRecycles.ContainsKey(obj.tag))
        {
            pool.onRecycles[obj.tag](obj.gameObject);
        }
        if (obj.onRecycle != null)
            obj.onRecycle(obj.gameObject);
        obj.transform.SetParent(pool.transform);
        obj.gameObject.SetActive(false);
        pool.gameObjectPool[obj.tag].Add(obj.gameObject);
    }

    public static void Clear()
    {
        if (!pool)
        {
            return;
        }

        foreach(var one in pool.gameObjectPool)
        {
            for(int i=0;i<one.Value.Count;i++)
            {
                one.Value[i].GetComponent<RecycleComponent>().IsInPool = false;
                GameObject.Destroy(one.Value[i]);
            }
        }
        pool.gameObjectPool = new Dictionary<string, List<GameObject>>();
    }
    public static void RegisterTakeOutEvent(string tag, Action<GameObject> action)
    {
        pool.onTakeOuts[tag]= action;
    }
    public static void RegisterRecycleEvent(string tag, Action<GameObject> action)
    {
        pool.onRecycles[tag] = action;
    }

    private void OnDestroy()
    {
        Clear();
    }
}
