using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReadCondition : MonoBehaviour
{

    public Text T_MaxHP;
    public Text T_CurrentHP;
    public float MaxHP;
    public float CurrentHP;
    public Slider HPslider;
    public Text T_CharacterName;

    //300*50

    // Use this for initialization
    void Start()
    {
        //this.T_MaxHP.text = StateStaticComponent.m_currentEntity.GetComponent<PropertyComponent>().HP.ToString();
        //this.T_CurrentHP.text = StateStaticComponent.m_currentEntity.GetComponent<DeadComponent>().hp.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (StateStaticComponent.m_currentEntity == null || StateStaticComponent.m_currentEntity.GetComponent<BlockInfoComponent>().m_blockType == BlockType.Enemy)
        {
            
            return;
        }
        this.T_CharacterName.text = StateStaticComponent.m_currentEntity.GetComponent<BlockInfoComponent>().m_blockName;

        this.T_MaxHP.text = StateStaticComponent.m_currentEntity.GetComponent<PropertyComponent>().HP.ToString();
        MaxHP = StateStaticComponent.m_currentEntity.GetComponent<PropertyComponent>().HP;
        HPslider.maxValue = MaxHP;

        this.T_CurrentHP.text = StateStaticComponent.m_currentEntity.GetComponent<DeadComponent>().hp.ToString();
        CurrentHP = StateStaticComponent.m_currentEntity.GetComponent<DeadComponent>().hp;

        HPslider.value = CurrentHP;

        if (CurrentHP < 0)
        {
            this.T_CurrentHP.text = "0";
            HPslider.value = 0;
        }
    }

}
