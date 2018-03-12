using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    
    public void OnClick()
    {

        if (StateStaticComponent.m_currentEntity != null && StateStaticComponent.m_currentEntity.GetComponent<ItemComponent>() != null)
        {
            StateStaticComponent.m_currentEntity.GetComponent<ItemComponent>().current = (ItemType)int.Parse(gameObject.name);
            StateStaticComponent.m_currentEntity.GetComponent<InputComponent>().leftButtonDown = false;
        }
        UiManager.uiManager.CloseBag();
    }
}
