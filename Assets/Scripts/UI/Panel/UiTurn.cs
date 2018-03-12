using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiTurn : MonoBehaviour {
    float i = 0;
	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {
        if (i > 0.5)
        {
            if (StateStaticComponent.m_currentEntity != null)
                transform.GetComponentInChildren<Text>().text = "当前行动方："+ StateStaticComponent.m_currentEntity.GetComponent<BlockInfoComponent>().m_blockType.ToString();
            i++;
        }
        i += Time.deltaTime;
    }
}
