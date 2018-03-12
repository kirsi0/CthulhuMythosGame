using System;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.IO;

using UnityEngine;

namespace Skyunion
{
	public class SceneManager : GameModule<SceneManager>
	{
		Camera mMainCamera;
		Dictionary<string, GameObject> mAllScenes = new Dictionary<string, GameObject> ();
		private BaseScene mCurrentScene = null;

		public BaseScene currentScene {
			get {
				return mCurrentScene;
			}
		}

		void Start ()
		{
			mMainCamera = GameObject.FindObjectOfType<Camera> ();
		}

		public void ShowScene<T> (Dictionary<string, object> varList = null) where T : BaseScene
		{
			if (mCurrentScene != null) {
				mCurrentScene.gameObject.SetActive (false);
				mCurrentScene = null;
			}

			string name = typeof (T).ToString ();
			GameObject uiObject;
			if (!mAllScenes.TryGetValue (name, out uiObject)) {
				string perfbName = "Scenes/" + typeof (T).ToString ();
				Debug.Log (perfbName);
				GameObject perfb = AssetsManager.LoadPrefabs<GameObject> (perfbName);
				uiObject = GameObject.Instantiate (perfb);
				uiObject.name = name;
				uiObject.AddComponent<T> ();
				uiObject.transform.SetParent (transform);

				mAllScenes.Add (name, uiObject);
			} else {
				uiObject.SetActive (true);
			}

			if (uiObject) {
				T scene = uiObject.GetComponent<T> ();
				if (varList != null)
					scene.mUserData = varList;

				mCurrentScene = scene;

				uiObject.SetActive (true);
			}
		}

		public void CloseScene ()
		{
			if (mCurrentScene) {
				mCurrentScene.gameObject.SetActive (false);
				mCurrentScene = null;
			}
		}
	}
}