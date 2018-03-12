using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Skyunion;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UINoteBookPanel : UIPanel
{

	// Use this for initialization
	void Start ()
	{
		AddButtonClick ("Close", CloseNote);

	}

	// Update is called once per frame
	void Update ()
	{

	}

	void CloseNote ()
	{
		UIManager.Instance ().ClosePanel<UINoteBookPanel> ();
	}
}
