using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Skyunion;

public class UIMainMenu : UIMenu
{

	virtual public void Start ()
	{
		AddButtonClick ("StartButton", StartGame);
	}

	void StartGame ()
	{

		//Destroy(GameObject.Find("MCamera"));
		UIManager.Instance ().ClosePanel<UIMainMenu> ();
		SceneManager.Instance ().ShowScene<ParkBattleScene> ();


	}
}