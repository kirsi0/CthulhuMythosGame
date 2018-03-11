using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiSkill : MonoBehaviour {

    private GameObject backGround;
    private BasicEntity current;
	// Use this for initialization
	void Awake () {
        backGround = transform.parent.Find("BackGround").gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		if(current!=StateStaticComponent.m_currentEntity)
        {
            current = StateStaticComponent.m_currentEntity;
            Clear();
            AddClick(current);
        }
	}

    public void Clear()
    {
        List<GameObject> childList = new List<GameObject>();
        int childCount = transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            GameObject child = transform.GetChild(i).gameObject;
            childList.Add(child);
        }
        for (int i = 0; i < childCount; i++)
        {
            DestroyImmediate(childList[i]);
        }
    }

    

    public void AddClick(BasicEntity entity)
    {
        List<ComponentType> m_components = entity.GetComponent<AbilityComponent>().m_temporaryAbility;
        for(int i=0;i<m_components.Count&&m_components[i]!=ComponentType.Input;i++)
        {
            ComponentType item = m_components[i];
            GameObject skillButton = new GameObject((i+1).ToString());
            skillButton.transform.SetParent(gameObject.transform);
            skillButton.gameObject.transform.position = backGround.transform.GetChild(i).position;

            
            Button button = skillButton.AddComponent<Button>();
            SkillButton skill = skillButton.AddComponent<SkillButton>();
            switch (item)
            {
                case ComponentType.CheerUp:
                    {
                        skillButton.AddComponent<Image>().sprite = Resources.Load<Sprite>("CheerUp");
                        break;
                    }
                case ComponentType.Move:
                    {
                        skillButton.AddComponent<Image>().sprite = Resources.Load<Sprite>("Move");
                        break;
                    }
                case ComponentType.Attack:
                    {
                        skillButton.AddComponent<Image>().sprite = Resources.Load<Sprite>("Attack");
                        break;
                    }
                case ComponentType.Knock:
                    {
                        skillButton.AddComponent<Image>().sprite = Resources.Load<Sprite>("Knock");
                        break;
                    }
            }
            if (entity.GetComponent<AbilityComponent>().m_coldDown.ContainsKey(item))
                skillButton.GetComponent<Image>().color=Color.black;
            skillButton.transform.localScale = Vector3.one;
            button.onClick.AddListener(delegate () { skill.OnClick(); });
        }
    }
}
