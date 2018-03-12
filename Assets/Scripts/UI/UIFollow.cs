using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFollow : MonoBehaviour
{
	public string m_followName;
	public GameObject m_follow;
	public RectTransform m_rectTransform;
	public Vector3 m_offsetPos;


	// Use this for initialization
	void Start ()
	{

	}

	// Update is called once per frame
	void Update ()
	{
		Vector3 tarPos = m_follow.transform.position;
		Vector3 pos = Camera.main.WorldToScreenPoint (tarPos);
		if (gameObject.name == "UIPortraiture") {
			Debug.Log (pos);
		}
		if (pos.z > 0) {
			m_rectTransform.position = pos + m_offsetPos;
		}

	}
}
