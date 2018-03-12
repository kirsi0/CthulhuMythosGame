using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UIModule : MonoBehaviour
{
	DialogWriter dialogWriter;

	private void Start ()
	{
		Debug.Log ("comin ");
		dialogWriter = XMLUtility.LoadXMLEvent ();
		//AddGameObject ();
		LoadDialogEvent ("first");
	}

	public void LoadDialogEvent (string name)
	{
		for (int i = 0; i < dialogWriter.m_dialogEventList.Count; i++) {
			if (dialogWriter.m_dialogEventList [i].m_name == name) {
				//

				DialogPlayer.LoadDialogEvent (dialogWriter.m_dialogEventList [i]);
			}
		}

	}

	public void ShowDialogUI ()
	{
	}


}

