using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaveScene : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{

	}

	// Update is called once per frame
	void Update ()
	{

	}

	private void OnTriggerEnter (Collider other)
	{
		Skyunion.UIManager.Instance ().ClosePanel<UIInvestigateScenePanel> ();
		GameObject.Find ("RigidBodyFPSController").GetComponent<UnityStandardAssets.Characters.FirstPerson.RigidbodyFirstPersonController> ().mouseLook.SetCursorLock (false);

		Skyunion.SceneManager.Instance ().ShowScene<ParkBattleScene> ();

	}
}
