using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

using Skyunion;

public class DialogUI : UIPanel
{
	Text m_content;
	Text m_name;
	// Use this for initialization
	void Start ()
	{
		AddClickEvent ("Content", UpdateContent);
		AddClickEvent ("Background", UpdateContent);
		AddClickEvent ("Tachie", UpdateContent);


		m_content = transform.Find ("Content").gameObject.GetComponent<Text> ();
		m_name = transform.Find ("Name").gameObject.GetComponent<Text> ();

		if (DialogPlayer.IsReading ()) {
			m_content.text = DialogPlayer.LoadContent ();
			m_name.text = DialogPlayer.LoadName ();
		}
	}

	// Update is called once per frame
	void Update ()
	{

	}

	void UpdateContent (BaseEventData eventData)
	{
		if (DialogPlayer.IsReading ()) {
			m_content.text = DialogPlayer.LoadContent ();
			m_name.text = DialogPlayer.LoadName ();
		} else {
			UIManager.Instance ().ClosePanel<DialogUI> ();
		}

	}
}
