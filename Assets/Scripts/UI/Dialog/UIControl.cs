using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIControl : MonoBehaviour
{
    public GameObject BagMenu;
    public GameObject BagList;

    // Use this for initialization
    void Start()
    {
        Button button = GetComponent<Button>();
        button.onClick.AddListener(delegate () { this.OnBagClick(); });

    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnBagClick()
    {
        if (BagMenu.activeSelf)
        {
            OnExit();
        }
        else
        {
            List<ItemType> item = StateStaticComponent.m_currentEntity.GetComponent<ItemComponent>().item;
            foreach (ItemType t in item)
            {
                GameObject button = new GameObject();
                button.transform.parent = BagList.transform;
                switch (t)
                {
                    case ItemType.Bottle:
                        {
                            Button buttonBottle = button.AddComponent<Button>();
                            button.AddComponent<Image>().sprite = Resources.Load<Sprite>("bottle");
                            buttonBottle.onClick.AddListener(delegate () { OnBottleClick(); });
                            break;
                        }
                    case ItemType.HealthPotion:
                        {
                            Button buttonCar = button.AddComponent<Button>();
                            button.AddComponent<Image>().sprite = Resources.Load<Sprite>("yaoshui");
                            buttonCar.onClick.AddListener(delegate () { OnHealthPointClick(); });
                            break;
                        }
                    case ItemType.Bomb:
                        {
                            Button buttonBomb = button.AddComponent<Button>();
                            button.AddComponent<Image>().sprite = Resources.Load<Sprite>("bottle");
                            buttonBomb.onClick.AddListener(delegate () { OnBombClick(); });
                            break;
                        }
                }
            }
            BagMenu.SetActive(true);
        }
    }

    public void OnExit()
    {
        List<GameObject> childList = new List<GameObject>();
        int childCount = BagList.transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            GameObject child = BagList.transform.GetChild(i).gameObject;
            childList.Add(child);
        }
        for (int i = 0; i < childCount; i++)
        {
            DestroyImmediate(childList[i]);
        }
        BagMenu.SetActive(false);
    }

    public void OnBottleClick()
    {
       StateStaticComponent.m_currentEntity.GetComponent<ItemComponent>().current = ItemType.Bottle;
       OnExit();
    }
    public void OnHealthPointClick()
    {
        StateStaticComponent.m_currentEntity.GetComponent<ItemComponent>().current = ItemType.HealthPotion;
        OnExit();
    }
    public void OnBombClick()
    {
        StateStaticComponent.m_currentEntity.GetComponent<ItemComponent>().current = ItemType.Bomb;
        OnExit();
    }
}
