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
                transform.Rotate(0,45,0,Space.World);

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
        float angle = transform.rotation.eulerAngles.y * 3.1415f / 180;
        
        transform.Translate ( RotatePos( Vector3.right * h,angle) * Time.deltaTime * speed, Space.World);
		transform.Translate (RotatePos( Vector3.forward * v,angle) * Time.deltaTime * speed, Space.World);
		if (Input.GetButtonDown ("Fouce")) {
			CameraEffect.cameraType = CameraType.Fouce;
		}
	}
    private Vector3 RotatePos(Vector3 pos, float angle)
    {
        Vector3 newPos = pos;
        newPos.z = pos.z * Mathf.Cos(angle) - pos.x * Mathf.Sin(angle);
        newPos.x = pos.z * Mathf.Sin(angle) + pos.x * Mathf.Cos(angle);
        return newPos;
    }
}
