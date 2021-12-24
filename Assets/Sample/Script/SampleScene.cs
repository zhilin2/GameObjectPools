using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleScene : MonoBehaviour
{
    private List<GameObject> list=new List<GameObject>();
    public Transform root;

    public void CreateCube()
    {
        GameObject obj1= MyObject.Instantiate(Resources.Load("Cube")) as GameObject;
        obj1.transform.SetParent(root);
        obj1.transform.localPosition = new Vector3(Random.Range(-8f, 8f), Random.Range(-5f, 5f),0);
        obj1.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        list.Add(obj1);
    }
    public void DeleteCube()
    {
        if(list.Count>0)
        {
            GameObject obj1 = list[0];
            list.RemoveAt(0);
            MyObject.Destroy(obj1);


        }

    }
}
