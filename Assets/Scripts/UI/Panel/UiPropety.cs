using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiPropety : MonoBehaviour {

    BasicEntity currentRole=null;
    float i = 0;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (StateStaticComponent.m_currentEntity != null)
        {
            if (StateStaticComponent.m_currentEntity.GetComponent<BlockInfoComponent>().m_blockType == BlockType.Player)
                currentRole = StateStaticComponent.m_currentEntity;
        }
        LoadInfo();
    }
    public void Clear()
    {

    }

    public void LoadInfo()
    {
        if (currentRole == null)
            return;
        PropertyComponent c_Pro = currentRole.GetComponent<PropertyComponent>();
        DeadComponent c_Dead = currentRole.GetComponent<DeadComponent>();
        StateComponent c_State = currentRole.GetComponent<StateComponent>();
        transform.Find("HP").GetComponent<Text>().text = "HP: " + c_Dead.hp.ToString()+"/"+c_Pro.HP.ToString();
        transform.Find("AP").GetComponent<Text>().text = "AP: " + c_State.m_actionPoint.ToString()+"/"+c_Pro.AP.ToString();
        transform.Find("Name").GetComponent<Text>().text = "Name: " + c_Pro.name;
        transform.Find("ATK").GetComponent<Text>().text = "ATK: " + c_Pro.STR.ToString();
        transform.Find("SPD").GetComponent<Text>().text = "SPD: " + c_Pro.SPD.ToString();
    }

}
