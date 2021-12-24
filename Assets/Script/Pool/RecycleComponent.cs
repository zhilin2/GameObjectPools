using UnityEngine;
using System;
public class RecycleComponent : MonoBehaviour
{
    new public string tag;
    [HideInInspector]
    public bool IsInPool;
    /// <summary>
    /// 回收
    /// </summary>
    public Action<GameObject> onRecycle;
    /// <summary>
    ///取出
    /// </summary>
    public Action<GameObject> onTakeOut;
    private void OnDestroy()
    {
        if(IsInPool)
        {
            Debug.LogError("我都被回收了，你还想把我强制删除，肯定会出问题的！！！     " + gameObject.name+"    parent:"+transform.parent.name);
        }
    }
}

