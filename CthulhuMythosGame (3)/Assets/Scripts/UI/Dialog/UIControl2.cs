using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIControl2 : MonoBehaviour
{
    public InputComponent a;
    public InputComponent b;
    public InputComponent c;
    public InputComponent d;
    public InputComponent e;
    public GameObject skill;
    // Use this for initialization
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(delegate () { this.OnClick(); });
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnClick()
    {
        if (skill.transform.childCount > 0)
        {
            if (skill.activeSelf)
            {
                OnBottonDown();
            }
            return;
        }
            
        List<BasicComponent> m_components = StateStaticComponent.m_currentEntity.GetComponent<BasicEntity>().m_components;
        foreach (BasicComponent t in m_components)
        {
            ComponentType item = t.m_componentType;
            GameObject button = new GameObject();
            button.transform.parent = skill.transform;
            switch (item)
            {
                case ComponentType.CheerUp:
                    {
                        Button buttonCheerUp = button.AddComponent<Button>();
                        //button.AddComponent<Image>();
                        button.AddComponent<Image>().sprite = Resources.Load<Sprite>("6");
                        buttonCheerUp.onClick.AddListener(delegate () { this.OnClick1(); });
                        break;
                    }
                case ComponentType.Move:
                    {
                        Button buttonMove = button.AddComponent<Button>();
                        //button.AddComponent<Image>();
                        button.AddComponent<Image>().sprite = Resources.Load<Sprite>("7");
                        buttonMove.onClick.AddListener(delegate () { this.OnClick2(); });
                        break;
                    }
                case ComponentType.Attack:
                    {
                        Button buttonAttack = button.AddComponent<Button>();
                        //button.AddComponent<Image>();
                        button.AddComponent<Image>().sprite = Resources.Load<Sprite>("8");
                        buttonAttack.onClick.AddListener(delegate () { this.OnClick3(); });
                        break;
                    }
                case ComponentType.Knock:
                    {
                        Button buttonKnock = button.AddComponent<Button>();
                        //button.AddComponent<Image>();
                        button.AddComponent<Image>().sprite = Resources.Load<Sprite>("9");
                        buttonKnock.onClick.AddListener(delegate () { this.OnClick4(); });
                        break;
                    }
                case ComponentType.Item:
                    {
                        Button buttonItem = button.AddComponent<Button>();
                        //button.AddComponent<Image>();
                        button.AddComponent<Image>().sprite = Resources.Load<Sprite>("10");
                        buttonItem.onClick.AddListener(delegate () { this.OnClick5(); });
                        break;
                    }
            }

        }

        skill.SetActive(true);
    }

    public void OnBottonDown()
    {
        List<GameObject> childList = new List<GameObject>();
        int childCount = skill.transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            GameObject child = skill.transform.GetChild(i).gameObject;
            childList.Add(child);
        }
        for (int i = 0; i < childCount; i++)
        {
            DestroyImmediate(childList[i]);
        }
        skill.SetActive(false);
    }


    public void OnClick1()
    {
        a = StateStaticComponent.m_currentEntity.GetComponent<InputComponent>();
        a.currentKey = 1;
        int b;
        b = a.currentKey;
        //Debug.Log(b);
        OnBottonDown();
    }
    public void OnClick2()
    {
        b = StateStaticComponent.m_currentEntity.GetComponent<InputComponent>();
        b.currentKey = 2;
        OnBottonDown();
    }
    public void OnClick3()
    {
        c = StateStaticComponent.m_currentEntity.GetComponent<InputComponent>();
        c.currentKey = 3;
        OnBottonDown();
    }
    public void OnClick4()
    {
        d = StateStaticComponent.m_currentEntity.GetComponent<InputComponent>();
        d.currentKey = 4;
        OnBottonDown();
    }
    public void OnClick5()
    {
        e = StateStaticComponent.m_currentEntity.GetComponent<InputComponent>();
        e.currentKey = 5;
        OnBottonDown();
    }
}
