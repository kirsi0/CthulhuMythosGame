using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaUIControl : MonoBehaviour {
    public GameObject ChaCondition;
    public GameObject PropertyButton;
    public GameObject ChaProterty;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void ChaUIopen()
    {
        ChaCondition.SetActive(true);
        ChaProterty.SetActive(false);
        PropertyButton.SetActive(true);
    }

    void ChaUIexit()
    {
        ChaCondition.SetActive(false);
        ChaProterty.SetActive(false);
        PropertyButton.SetActive(false);
    }
}
