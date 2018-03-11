using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReadProperty : MonoBehaviour {

    //text
    public Text T_SPD;
    public Text T_moveSPD;
    public Text T_STR;
    public Text T_HP;
    public Text T_San;
    public Text T_AP;
    public Text T_HRZ;
    public Text T_agility;

    public Text CharacterName;

    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        this.T_SPD.text = StateStaticComponent.m_currentEntity.GetComponent<PropertyComponent>().SPD.ToString();
        this.T_moveSPD.text = StateStaticComponent.m_currentEntity.GetComponent<PropertyComponent>().moveSpd.ToString();
        this.T_AP.text = StateStaticComponent.m_currentEntity.GetComponent<PropertyComponent>().AP.ToString();
        this.T_San.text = StateStaticComponent.m_currentEntity.GetComponent<PropertyComponent>().San.ToString();
        this.T_HRZ.text = StateStaticComponent.m_currentEntity.GetComponent<PropertyComponent>().HRZ.ToString();
        this.T_HP.text = StateStaticComponent.m_currentEntity.GetComponent<PropertyComponent>().HP.ToString();
        this.T_agility.text = StateStaticComponent.m_currentEntity.GetComponent<PropertyComponent>().m_agility.ToString();
        this.CharacterName.text = StateStaticComponent.m_currentEntity.GetComponent<BlockInfoComponent>().m_blockName;
    }
}
