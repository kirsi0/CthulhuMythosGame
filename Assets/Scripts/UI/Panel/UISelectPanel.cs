using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UISelectPanel : Skyunion.UIPanel
{

	GameObject m_selectionGo;

	Transform m_selectionTf;


	// Use this for initialization
	void Start ()
	{

	}

	// Update is called once per frame
	void Update ()
	{

	}

	public void InitSelect (List<string> selections)
	{
		m_selectionTf = transform.Find ("Select");
		m_selectionGo = m_selectionTf.gameObject;

		m_selectionGo.name = selections [0];
		m_selectionTf.Find ("SelectText").GetComponent<Text> ().text = selections [0];

		if (selections.Count > 1) {
			for (int i = 1; i < selections.Count; i++) {
				GameObject go = GameObject.Instantiate (m_selectionGo);

				go.transform.SetParent (m_selectionTf.parent);
				go.transform.localScale = new Vector3 (1, 1, 1);
				go.AddComponent<SendSelected> ();
				go.GetComponent<RectTransform> ().position = m_selectionTf.GetComponent<RectTransform> ().position + new Vector3 (0, 90, 0);
				go.transform.Find ("SelectText").GetComponent<Text> ().text = selections [i];
				go.name = selections [i];
			}
		}

		for (int i = 0; i < selections.Count; i++) {
			AddButtonClick (selections [i], Select);
		}

		GameObject.Find ("RigidBodyFPSController").GetComponent<UnityStandardAssets.Characters.FirstPerson.RigidbodyFirstPersonController> ().mouseLook.SetCursorLock (false);

	}

	public void Select ()
	{
		CancelInvoke ();
		Invoke ("CallDialogPlayer", 0.5f);

	}

	public void CallDialogPlayer ()
	{
		DialogPlayer.PlaySelection ();
	}
}
