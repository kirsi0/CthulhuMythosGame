using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Skyunion;

public class UIMainMenuPanel : UIMenu
{

	virtual public void Start ()
	{
		AddButtonClick ("StartButton", StartGame);
	}

	void StartGame ()
	{


		UIManager.Instance ().ClosePanel<UIMainMenuPanel> ();

		SceneManager.Instance ().ShowScene<ResidenceInvestigateScene> ();
		UIManager.Instance ().ShowPanel<UIInvestigateScenePanel> ();

		Destroy (GameObject.Find ("MCamera"));
	}
}