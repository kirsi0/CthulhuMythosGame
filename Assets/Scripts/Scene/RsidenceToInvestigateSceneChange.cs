using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RsidenceToInvestigateSceneChange : MonoBehaviour
{
	//调查出现新的难度系数
	public int diff = 9;
	//接收到的所选择的选项
	public string m_selected;
	// Use this for initialization
	void Start ()
	{

	}

	// Update is called once per frame
	void Update ()
	{

	}

	private void OnTriggerEnter (Collider other)
	{


		if (TmpDate.m_investigatePoint >= diff) {
			DialogPlayer.Load ("StewardSuccess");
			Debug.Log ("-------FINISH!!!!!!!!--------");
		} else {
			DialogPlayer.Load ("StewardFailure");
			Debug.Log ("-------FINISH!!!!!!!!--------");
		}

		GetComponent<Animation> ().Play ("OpenGateDoor");
	}
	//接收收到的选项
	public void Select (string select)
	{
		m_selected = select;
		Debug.Log (m_selected);
	}
}
