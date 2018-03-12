using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Skyunion
{
	public class GameRoot : MonoSingleton<GameRoot>
	{
		// Use this for initialization
		void Start ()
		{
			AddGameObject<DBService> ();
			//AddGameObject<NetService>();
			AddGameObject<SoundService> ();
			AddGameObject<UIManager> ();
			//AddGameObject<LogicManager>();
			AddGameObject<SceneManager> ();

		}

		// Update is called once per frame
		void Update ()
		{
		}
	}
}
