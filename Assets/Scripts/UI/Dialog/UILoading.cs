using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Skyunion;

public class UILoading : UIDialog
{
	private Slider loadingBar;
	private float time = 2.0f;
	// Use this for initialization
	void Start ()
	{
		loadingBar = transform.Find ("loadingbar").GetComponent<Slider> ();
		SoundService.Instance ().PlayMusic ("mainMainMusic.mp3");

		// 需要初始化的逻辑
		//LogicManager.Instance().AddLogic<LoginLogic>();
	}

	// Update is called once per frame
	void Update ()
	{
		if (loadingBar.value < 1.0f)
			loadingBar.value = loadingBar.value + Time.deltaTime * 1.0f / time;
		else {
			Skyunion.UIManager.Instance ().CloseUI ();
			//SceneManager.Instance().ShowScene<BattleScene>();
		}
	}
}