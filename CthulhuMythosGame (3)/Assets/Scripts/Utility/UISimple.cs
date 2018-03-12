using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISimple : MonoBehaviour
{

    GameObject panel;
    // Use this for initialization
    void Start()
    {
        GameObject r = new GameObject("red");
        GameObject g = new GameObject("green");
        GameObject b = new GameObject("blue");
        r.transform.parent = transform;
        g.transform.parent = transform;
        b.transform.parent = transform;
        panel = transform.Find("Canvas").gameObject;
    }

    private void Update()
    {
        if (StateStaticComponent.m_currentEntity != null 
            && StateStaticComponent.m_currentEntity.GetComponent<BlockInfoComponent>().m_blockType == BlockType.Player&&
            StateStaticComponent.m_currentEntity.GetComponent<InputComponent>()!=null)
        {
            if (!panel.activeSelf)
            {
                panel.SetActive(true);
            }
        }
        else
        {
            if (panel.activeSelf)
            {
                panel.SetActive(false);
            }
        }
    }

    public void ShowUI(List<Vector3> list, int Type)
    {
        Type--;
        Transform t = transform.GetChild(Type + 1);
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
        for (int i = 1; i < transform.childCount; i++)
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
