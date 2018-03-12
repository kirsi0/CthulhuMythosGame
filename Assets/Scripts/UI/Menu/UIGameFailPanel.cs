using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Skyunion;

<<<<<<< HEAD
public class UIGameFail : UIMenu
=======
public class UIGameFailPanel : UIMenu
>>>>>>> temp
{



	virtual public void Start ()
	{
		AddButtonClick ("FinishButton", FinishGame);
	}

	void FinishGame ()
	{
		SceneManager.Instance ().CloseScene ();
<<<<<<< HEAD
		UIManager.Instance ().ClosePanel<UIGameComplete> ();
		UIManager.Instance ().ShowPanel<UIMainMenu> ();
=======
		UIManager.Instance ().ClosePanel<UIGameCompletePanel> ();
		UIManager.Instance ().ShowPanel<UIMainMenuPanel> ();
>>>>>>> temp

	}
}
