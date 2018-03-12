using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Skyunion;

public class BattleSceneRoot : MonoSingleton<BattleSceneRoot>
{
	// Use this for initialization
	void Start ()
	{
		AddGameObject<UIManager> ();
	}

	// Update is called once per frame
	void Update ()
	{
	}
}
