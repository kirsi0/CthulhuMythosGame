using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiTips : MonoBehaviour {

    public int maxEnemyNum=0;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (maxEnemyNum == 0 && StateStaticComponent.enemyActionList != null)
            maxEnemyNum = StateStaticComponent.enemyActionList.Count;
        int currentEnemyNum = StateStaticComponent.enemyActionList.Count;
        transform.Find("enemyNum").GetComponent<Text>().text = currentEnemyNum + "/" + maxEnemyNum;
	}

    void AddInof()
    {
        
    }
}
