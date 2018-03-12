using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastDetection : MonoBehaviour
{
	public LayerMask m_mask;

	public float m_maxDistance = 2;
	private Scene m_scene;

	private string m_oldName;

	public bool m_openDetection = true;

	// Use this for initialization
	void Start ()
	{
		m_scene = GameObject.Find ("Scene").GetComponent<Scene> ();
	}


	void Update ()
	{
		//Debug.Log (m_openDetection);
		// 以摄像机所在位置为起点，创建一条向下发射的射线  
		Ray ray = new Ray (transform.position, transform.forward);
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit, m_maxDistance, m_mask)) {

			if (Input.GetMouseButtonDown (0) && m_openDetection == true) {


				m_scene.ShowItemInfo (hit.transform.parent.name);
				//m_scene.ShowItemUI (hit.transform.parent.name);
			}

			ShowUI (hit.transform.parent.name);
			// 如果射线与平面碰撞，打印碰撞物体信息  
			//Debug.Log ("碰撞对象: " + hit.transform.parent.name);
			//Debug.Log  ("碰撞距离：" + hit.distance);
			//m_scene.ShowItemUI (hit.transform.parent.name);
			// 在场景视图中绘制射线  
			Debug.DrawLine (ray.origin, hit.point, Color.red);
		}
	}

	void ShowUI (string aName)
	{
		if (aName != m_oldName) {
			if (m_oldName != null) {
				m_scene.ClsoeItemUI (m_oldName);
			}
			m_scene.ShowItemUI (aName);
			m_oldName = aName;
		}
	}
	//是否开启射线检测
	public void OpenDetection ()
	{
		m_openDetection = true;
	}

	public void CloseDetection ()
	{
		m_openDetection = false;
	}
}
