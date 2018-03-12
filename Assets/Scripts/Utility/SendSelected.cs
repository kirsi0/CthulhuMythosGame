using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SendSelected : MonoBehaviour
{
	public Transform target;
	// Use this for initialization
	void Start ()
	{
		target = GameObject.Find ("Door").transform.Find ("default");

		Button btn = this.GetComponent<Button> ();
		btn.onClick.AddListener (OnClick);
	}

	// Update is called once per frame
	void Update ()
	{

	}
	//点击的时候把信息发送出去
	void OnClick ()
	{
		DialogPlayer.m_selected = name;
		target.GetComponent<RsidenceToInvestigateSceneChange> ().Select (name);

	}
}
