using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Skyunion;

public class UIGameFailPanel : UIMenu
{



	virtual public void Start ()
	{
		AddButtonClick ("FinishButton", FinishGame);
	}

	void FinishGame ()
	{
		SceneManager.Instance ().CloseScene ();
		UIManager.Instance ().ClosePanel<UIGameCompletePanel> ();
		UIManager.Instance ().ShowPanel<UIMainMenuPanel> ();

	}
}
