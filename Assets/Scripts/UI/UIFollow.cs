using System.Collections;
using System.Collections.Generic;
using UnityEngine;
<<<<<<< HEAD

=======
//Ui一直跟随镜头
>>>>>>> temp
public class UIFollow : MonoBehaviour
{
	public string m_followName;
	public GameObject m_follow;
	public RectTransform m_rectTransform;
	public Vector3 m_offsetPos;

<<<<<<< HEAD

=======
	[Header ("Dont Open It")]
	public bool m_word = false;
>>>>>>> temp
	// Use this for initialization
	void Start ()
	{

	}

	// Update is called once per frame
	void Update ()
	{
<<<<<<< HEAD
		Vector3 tarPos = m_follow.transform.position;
		Vector3 pos = Camera.main.WorldToScreenPoint (tarPos);
		if (gameObject.name == "UIPortraiture") {
			Debug.Log (pos);
		}
		if (pos.z > 0) {
			m_rectTransform.position = pos + m_offsetPos;
		}

=======
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
		m_rectTransform.position = new Vector3 (-320, -250, 0);
>>>>>>> temp
	}
}
