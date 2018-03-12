using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Ui一直跟随镜头

public class UIFollow : MonoBehaviour
{
	public string m_followName;
	public GameObject m_follow;
	public RectTransform m_rectTransform;
	public Vector3 m_offsetPos;


	[Header ("Dont Open It")]
	public bool m_word = false;

	// Use this for initialization
	void Start ()
	{

	}

	// Update is called once per frame
	void Update ()
	{

		if (m_word == true) {
			Vector3 tarPos = m_follow.transform.position;
			Vector3 pos = Camera.main.WorldToScreenPoint (tarPos);

			if (pos.z > 0) {
				m_rectTransform.position = pos + m_offsetPos;
			}
		}


	}

	public void Show ()
	{
		m_word = true;
	}

	public void Close ()
	{
		m_word = false;

	}
}
