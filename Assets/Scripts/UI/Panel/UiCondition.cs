using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiCondition : MonoBehaviour {

    Transform current;
    Transform others;
    BasicEntity currentRole=null;
    float i = 0;
	// Use this for initialization
	void Start () {
        others = transform.Find("Friend");
        current = transform.Find("Current");
	}
	
	// Update is called once per frame
	void Update () {
        if (StateStaticComponent.m_currentEntity != null)
        {
            if (StateStaticComponent.m_currentEntity.GetComponent<BlockInfoComponent>().m_blockType == BlockType.Player)
                currentRole = StateStaticComponent.m_currentEntity;
        }
        if (i > 0.5)
        {
            updateInfo();
            i = 0;
        }
        i += Time.deltaTime;
	}

    public void Clear()
    {

    }

    public void AddClick(BasicEntity entity)
    {
        
    }

    private void updateInfo()
    {
        if (current == null)
            return;
        PropertyComponent c_Pro = currentRole.GetComponent<PropertyComponent>();
        DeadComponent c_Dead = currentRole.GetComponent<DeadComponent>();
        StateComponent c_State = currentRole.GetComponent<StateComponent>();
        if (c_Pro == null || c_Dead == null || c_State == null)
            return;
        current.Find("HP").GetComponent<Text>().text ="HP:"+ c_Dead.hp.ToString();
        current.Find("AP").GetComponent<Text>().text ="AP:"+ c_State.m_actionPoint.ToString();
        current.Find("Name").GetComponent<Text>().text = c_Pro.name;
        current.Find("HPSlider").GetComponent<Slider>().maxValue = c_Pro.HP;
        current.Find("HPSlider").GetComponent<Slider>().value = c_Dead.hp;
        current.Find("APSlider").GetComponent<Slider>().maxValue = c_Pro.AP;
        current.Find("APSlider").GetComponent<Slider>().value = c_State.m_actionPoint;

        foreach (BasicEntity role in StateStaticComponent.playerActionList)
        {
            if (role == currentRole)
                continue;
            PropertyComponent f_Pro = role.GetComponent<PropertyComponent>();
            DeadComponent f_Dead = role.GetComponent<DeadComponent>();
            if (f_Pro == null || f_Dead == null)
                continue;
            others.Find("HP").GetComponent<Text>().text ="HP:" + f_Dead.hp.ToString();
            others.Find("Name").GetComponent<Text>().text = f_Pro.name;
            others.Find("HPSlider").GetComponent<Slider>().maxValue = f_Pro.HP;
            others.Find("HPSlider").GetComponent<Slider>().value = f_Dead.hp;
        }
    }
}
