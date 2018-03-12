using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DontDestroy : MonoBehaviour
{

	void Awake ()
	{
		DontDestroyOnLoad (transform.gameObject);

	}


}

static class Console
{
	static Text logger = GameObject.Find ("Console").GetComponent<Text> ();



	static public void Log (string message)
	{
		if (logger == null) {
			GameObject go = GameObject.Find ("Console");
			if (go != null) {
				logger = go.GetComponent<Text> ();
			} else {
				Debug.Log ("Can`t Find Console GameObject!");
				return;
			}

		}
		logger.text += "\n" + message;
	}
}
