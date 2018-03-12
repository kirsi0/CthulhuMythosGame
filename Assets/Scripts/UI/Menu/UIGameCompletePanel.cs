using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Skyunion;
public class UIGameComplete : UIMenu
{


	virtual public void Start ()
	{
		AddButtonClick ("FinishButton", FinishGame);
	}

	void FinishGame ()
	{
		SceneManager.Instance ().CloseScene ();
		UIManager.Instance ().ClosePanel<UIGameComplete> ();
		UIManager.Instance ().ShowPanel<UIMainMenu> ();

	}
}
