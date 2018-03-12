﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TestPlayer : MonoBehaviour
{
    public float distance;
    public float angle = 45;

    private Vector3 m_CameraPosition;


	void Start ()
	{
	    Camera.main.transform.rotation = Quaternion.Euler(angle, 0, 0);
	    Camera.main.transform.position = transform.position - Camera.main.transform.forward*distance;
	}
	
	void Update () {
		m_CameraPosition = transform.position - Camera.main.transform.forward * distance;
	    Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, m_CameraPosition, Time.deltaTime*3f);

	}
}
