using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPropertyControl : MonoBehaviour {

    //page
    public GameObject ChaProperty;


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Enter()
    {
        ChaProperty.SetActive(true);
    }

    public void Exitt()
    {
        ChaProperty.SetActive(false);
    }
}
