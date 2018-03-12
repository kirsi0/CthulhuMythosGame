using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CameraType
{
	Null,
	Shack,
	Fouce
}

public static class CameraEffect
{
	static public CameraType cameraType = CameraType.Null;
	static public float time = 0;
}

public class CamaraController : MonoBehaviour
{

	public float speed = 3;

	// Use this for initialization
	void Start ()
	{

	}

	// Update is called once per frame
	void Update ()
	{
        if (Input.GetButtonDown("ChangeCamera"))
        {
                transform.Rotate(0,90,0,Space.World);

        }
		//Debug.Log(CameraEffect.cameraType);
		switch (CameraEffect.cameraType) {
		case CameraType.Null:
			Move ();
			break;
		case CameraType.Fouce:
			Fouce ();
			break;
		}
	}
	void Fouce ()
	{
		BasicEntity tgE = StateStaticComponent.m_currentEntity;
		if (tgE == null)
			return;
		Debug.Log (1111);
		Transform target = tgE.transform;
		transform.position = Vector3.MoveTowards (transform.position, target.position, speed * Time.deltaTime);
		if (Input.GetButtonDown ("Fouce")) {
			CameraEffect.cameraType = CameraType.Null;
		}
	}
	void Move ()
	{
		float h = Input.GetAxis ("CameraH");
		float v = Input.GetAxis ("CameraV");

		transform.Translate (Vector3.right * h * Time.deltaTime * speed, Space.Self);
		transform.Translate (Vector3.up * v * Time.deltaTime * speed, Space.Self);
		if (Input.GetButtonDown ("Fouce")) {
			CameraEffect.cameraType = CameraType.Fouce;
		}
	}
}
