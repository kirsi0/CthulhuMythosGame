using System;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.IO;

using UnityEngine;
using UnityEngine.UI;

namespace Skyunion
{
	public class UIManager : GameModule<UIManager>
	{
		private GameObject mDialog;
		private GameObject mPanel;
		private GameObject mBox;

		Dictionary<string, GameObject> mAllUIs = new Dictionary<string, GameObject> ();
		Queue<UIDialog> mDialogs = new Queue<UIDialog> ();
		private UIDialog mCurrentDialog = null;
		private UIBox mCurrentBox = null;

		void Start ()
		{
			mPanel = AddGameObject ("panel");
			mBox = AddGameObject ("box");
			mDialog = AddGameObject ("dialog");


			ShowUI<UILogo> ();
			//ShowPanel<UIInvestigateScenePanel> ();

		}

		public void ShowUI<T> (bool bPushHistory = true, Dictionary<string, object> varList = null) where T : UIDialog
		{
			if (mCurrentDialog != null) {
				mCurrentDialog.gameObject.SetActive (false);
				mCurrentDialog = null;
			}

			string name = typeof (T).ToString ();
			GameObject uiObject;
			if (!mAllUIs.TryGetValue (name, out uiObject)) {
				string perfbName = "UI/Dialog/" + typeof (T).ToString ();
				Debug.Log (perfbName);
				GameObject perfb = AssetsManager.LoadPrefabs<GameObject> (perfbName);
				uiObject = GameObject.Instantiate (perfb);
				uiObject.name = name;
				uiObject.AddComponent<T> ();
				uiObject.transform.SetParent (mDialog.transform);

				mAllUIs.Add (name, uiObject);
			} else {
				uiObject.SetActive (true);
			}

			if (uiObject) {
				T panel = uiObject.GetComponent<T> ();
				if (varList != null)
					panel.mUserData = varList;

				mCurrentDialog = panel;

				uiObject.SetActive (true);

				if (bPushHistory) {
					mDialogs.Enqueue (panel);
				}
			}
		}

		public void CloseUI ()
		{
			if (mCurrentDialog) {
				mCurrentDialog.gameObject.SetActive (false);
				mCurrentDialog = null;
			}
			mDialogs.Clear ();
		}

		public void ShowPanel<T> (Dictionary<string, object> varList = null) where T : UIPanel
		{
			string name = typeof (T).ToString ();

			Debug.Log (name);

			var panelTran = mPanel.transform.Find (name);
			GameObject uiObject;
			if (panelTran == null) {
				string perfbName = "UI/Panel/" + typeof (T).ToString ();
				GameObject perfb = AssetsManager.LoadPrefabs<GameObject> (perfbName);
				if (perfb == null) {
					Debug.Log ("UIPanel Can`t Find Perfab");
				}
				uiObject = GameObject.Instantiate (perfb);
				uiObject.name = name;
				uiObject.AddComponent<T> ();
				uiObject.transform.SetParent (mPanel.transform);
			} else {
				uiObject = panelTran.gameObject;
			}
			if (uiObject) {
				T panel = uiObject.GetComponent<T> ();
				if (varList != null)
					panel.mUserData = varList;

				uiObject.SetActive (true);
			}
		}
		public void ClosePanel<T> () where T : UIPanel
		{
			string name = typeof (T).ToString ();
			var panelTran = mPanel.transform.Find (name);
			if (panelTran != null)
				panelTran.gameObject.SetActive (false);
		}

		public void ShowBox<T> (Dictionary<string, object> varList = null) where T : UIBox
		{
			string name = typeof (T).ToString ();

			var panelTran = mBox.transform.Find (name);
			GameObject uiObject;
			if (panelTran == null) {
				string perfbName = "UI/Box/" + typeof (T).ToString ();
				GameObject perfb = AssetsManager.LoadPrefabs<GameObject> (perfbName);
				uiObject = GameObject.Instantiate (perfb);
				uiObject.name = name;
				uiObject.AddComponent<T> ();
				uiObject.transform.SetParent (mBox.transform);
			} else {
				uiObject = panelTran.gameObject;
			}
			if (uiObject) {
				T box = uiObject.GetComponent<T> ();
				if (varList != null)
					box.mUserData = varList;

				if (mCurrentBox)
					mCurrentBox.gameObject.SetActive (false);

				uiObject.SetActive (true);
				mCurrentBox = box;
			}
		}

		public void CloseBox ()
		{
			if (mCurrentBox)
				mCurrentBox.gameObject.SetActive (false);

			mCurrentBox = null;
		}
	}
}