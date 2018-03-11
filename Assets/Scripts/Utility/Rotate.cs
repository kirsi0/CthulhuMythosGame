using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
	private RectTransform m_rect;

	public int speed = 1;
	// Use this for initialization
	void Start ()
	{
		m_rect = GetComponent<RectTransform> ();

	}

	// Update is called once per frame
	void Update ()
	{
		m_rect.Rotate (Vector3.forward * speed);
	}
}
