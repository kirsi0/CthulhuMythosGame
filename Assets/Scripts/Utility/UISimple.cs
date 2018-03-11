using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISimple : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        GameObject r = new GameObject("red");
        GameObject g = new GameObject("green");
        GameObject b = new GameObject("blue");
        r.transform.parent = transform;
        g.transform.parent = transform;
        b.transform.parent = transform;
    }

    private void Update()
    {

    }

    public void ShowWornArea(BasicEntity entity)
    {
        List<Vector3> list = entity.GetComponent<MonitorComponent>().m_view;

        string name = "Prefabs/UI/Simple/red";
        VoxelBlocks map = GameObject.Find("Voxel Map").GetComponent<VoxelBlocks>();

        List<GameObject> childList = new List<GameObject>();
        int childCount = entity.transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            GameObject child = entity.transform.GetChild(i).gameObject;
            childList.Add(child);
        }
        for (int i = 0; i < childCount; i++)
        {
            DestroyImmediate(childList[i]);
        }

        if (list == null)
            return;
        foreach (var pos in list)
        {
            GameObject obj = (GameObject)Resources.Load(name);
            obj = Instantiate(obj);
            obj.transform.parent = entity.transform;
            obj.transform.position = map.LogToWorld(pos);
        }
    }

    public void ShowUI(List<Vector3> list, int Type)
    {
        Type--;
        Transform t = transform.GetChild(Type);
        string name = "Prefabs/UI/Simple/" + t.name;
        VoxelBlocks map = GameObject.Find("Voxel Map").GetComponent<VoxelBlocks>();

        List<GameObject> childList = new List<GameObject>();
        int childCount = t.transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            GameObject child = t.transform.GetChild(i).gameObject;
            childList.Add(child);
        }
        for (int i = 0; i < childCount; i++)
        {
            DestroyImmediate(childList[i]);
        }

        if (list == null)
            return;
        foreach (var pos in list)
        {
            GameObject obj = (GameObject)Resources.Load(name);
            obj = Instantiate(obj);
            obj.transform.parent = t;
            obj.transform.position = map.LogToWorld(pos);
        }
    }
    public void CloseUI()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            List<GameObject> childList = new List<GameObject>();
            int childCount = transform.GetChild(i).transform.childCount;
            for (int j = 0; j < childCount; j++)
            {
                GameObject child = transform.GetChild(i).transform.GetChild(j).gameObject;
                childList.Add(child);
            }
            for (int j = 0; j < childCount; j++)
            {
                DestroyImmediate(childList[j]);
            }
        }
    }

}
