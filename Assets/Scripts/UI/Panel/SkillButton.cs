using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillButton : MonoBehaviour {

    // Use this for initialization
    void Start () {
        transform.GetComponent<RectTransform>().sizeDelta =transform.parent.GetComponent<RectTransform>().sizeDelta;
	}
	
	// Update is called once per frame
	void Update () {


    }

    private void OnMouseEnter()
    {
        Debug.Log(1111);
    }

    public void OnClick()
    {
        if (StateStaticComponent.m_currentEntity != null && StateStaticComponent.m_currentEntity.GetComponent<InputComponent>() != null)
        {
            StateStaticComponent.m_currentEntity.GetComponent<InputComponent>().currentKey = int.Parse(gameObject.name);
            StateStaticComponent.m_currentEntity.GetComponent<InputComponent>().leftButtonDown = false;
        }
        UiManager.uiManager.CloseSkill();

    }
}
