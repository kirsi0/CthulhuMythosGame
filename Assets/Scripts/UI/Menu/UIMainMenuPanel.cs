using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Skyunion;

<<<<<<< HEAD
public class UIMainMenu : UIMenu
=======
public class UIMainMenuPanel : UIMenu
>>>>>>> temp
{

	virtual public void Start ()
	{
		AddButtonClick ("StartButton", StartGame);
	}

	void StartGame ()
	{

<<<<<<< HEAD
		//Destroy(GameObject.Find("MCamera"));
		UIManager.Instance ().ClosePanel<UIMainMenu> ();
		SceneManager.Instance ().ShowScene<ParkBattleScene> ();


=======

		UIManager.Instance ().ClosePanel<UIMainMenuPanel> ();

		SceneManager.Instance ().ShowScene<ResidenceInvestigateScene> ();
		UIManager.Instance ().ShowPanel<UIInvestigateScenePanel> ();

		Destroy (GameObject.Find ("MCamera"));
>>>>>>> temp
	}
}