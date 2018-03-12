﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

using Skyunion;


public class UIDialogPanel : UIPanel
{
	Text m_content;
	Text m_name;
	Image m_tachie;
	Image m_background;


	// Use this for initialization
	void Start ()
	{
		AddClickEvent ("Content", UpdateContent);
		AddClickEvent ("Background", UpdateContent);
		AddClickEvent ("Tachie", UpdateContent);



		m_content = transform.Find ("Content").GetComponent<Text> ();
		m_name = transform.Find ("Name").GetComponent<Text> ();
		m_tachie = transform.Find ("Tachie").GetComponent<Image> ();
		m_background = transform.Find ("Background").GetComponent<Image> ();

		InitDialog ();
	}

	public void InitDialog ()
	{
		if (DialogPlayer.IsReading ()) {
			UpdateDialog ();
		}
	}

	// Update is called once per frame
	void Update ()
	{

	}

	void UpdateContent (BaseEventData eventData)
	{
		if (DialogPlayer.IsReading ()) {

			UpdateDialog ();
		} else {

			GameObject.Find ("RigidBodyFPSController").GetComponent<UnityStandardAssets.Characters.FirstPerson.RigidbodyFirstPersonController> ().mouseLook.SetCursorLock (true);


			UIManager.Instance ().ClosePanel<UIDialogPanel> ();
			Camera.main.GetComponent<RayCastDetection> ().OpenDetection ();
		}

	}

	void UpdateDialog ()
	{
		m_content.text = DialogPlayer.LoadContent ();
		m_name.text = DialogPlayer.LoadName ();
		m_tachie.sprite = DialogPlayer.LoadTachie ();
		m_background.sprite = DialogPlayer.LoadBackground ();
	}

}
