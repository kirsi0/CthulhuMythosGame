using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof (PropertyComponent))]
public class EnemyPatrolEditor : Editor
{

	//所扩充的属性组件
	PropertyComponent m_prop;

	PatrolData m_patrolData;

	Vector3 m_point;

	public override void OnInspectorGUI ()
	{
		base.OnInspectorGUI ();

		EditorGUILayout.BeginVertical ();
		//选择自定义的指示物
		m_patrolData.m_referent = (GameObject)EditorGUILayout.ObjectField ("referent", m_patrolData.m_referent, typeof (GameObject), false);

		EditorGUILayout.BeginHorizontal ();

		m_point = EditorGUILayout.Vector3Field ("New Pos", m_point);
		if (GUILayout.Button ("Add")) {
			m_patrolData.m_patrolPoint.Add (m_point);
			RefreshReferent ();
		}
		EditorGUILayout.EndHorizontal ();

		for (int i = 0; i < m_patrolData.m_patrolPoint.Count; i++) {
			EditorGUILayout.BeginHorizontal ();
			Vector3 newPoint = m_patrolData.m_patrolPoint [i];
			newPoint = EditorGUILayout.Vector3Field ("path" + i, newPoint);
			if (newPoint != m_patrolData.m_patrolPoint [i]) {
				m_patrolData.m_patrolPoint [i] = newPoint;
				RefreshReferent ();
			}
			if (GUILayout.Button ("Delete")) {
				ClearSpecialReferent (i);
			}
			EditorGUILayout.EndHorizontal ();

		}

		EditorGUILayout.EndVertical ();
	}

	private void OnEnable ()
	{

		m_prop = target as PropertyComponent;


		//初始化所需的数据
		InitDate ();
		UpdatePatrolReferent ();
	}

	private void OnDisable ()
	{
		ClearReferent ();
	}

	//通过外部数据更新组件中的数据，然后重新绘制巡逻点
	void RefreshReferent ()
	{

		V3ListTo (m_patrolData.m_patrolPoint, m_prop.m_patrolPoint);
		ClearReferent ();
		V3ListTo (m_prop.m_patrolPoint, m_patrolData.m_patrolPoint);
		UpdatePatrolReferent ();
	}
	//清除所有的go实例和数据
	void ClearReferent ()
	{
		for (int i = 0; i < m_patrolData.m_patrolReferent.Count; i++) {
			DestroyImmediate (m_patrolData.m_patrolReferent [i]);
		}
		m_patrolData.m_patrolReferent.Clear ();
		m_patrolData.m_patrolPoint.Clear ();
	}

	void ClearSpecialReferent (int i)
	{
		DestroyImmediate (m_patrolData.m_patrolReferent [i]);
		m_patrolData.m_patrolReferent.RemoveAt (i);
		m_patrolData.m_patrolPoint.RemoveAt (i);
		RefreshReferent ();
	}

	void InitDate ()
	{
		if (m_prop.GetComponent<PatrolData> () == null) {
			//建立引用

			m_patrolData = m_prop.gameObject.AddComponent<PatrolData> ();
			m_patrolData.m_voxelMap = GameObject.Find ("Voxel Map").GetComponent<VoxelMap> ();
		} else {

			m_patrolData = m_prop.GetComponent<PatrolData> ();
		}

		V3ListTo (m_prop.m_patrolPoint, m_patrolData.m_patrolPoint);
	}

	void V3ListTo (List<Vector3> from, List<Vector3> to)
	{
		to.Clear ();
		foreach (Vector3 v in from) {
			to.Add (v);
		}
	}
	//显示所有的标志物
	private void UpdatePatrolReferent ()
	{

		if (m_patrolData != null && m_patrolData.m_referent != null) {

			foreach (Vector3 point in m_patrolData.m_patrolPoint) {
				ShowReferent (point);
			}
		}
	}
	//显示单一的标志物
	void ShowReferent (Vector3 logicPos)
	{
		Vector3 parentPos = m_patrolData.m_voxelMap.transform.position;

		Vector3 upperBlock = new Vector3 (m_patrolData.m_voxelMap.blockSize.x, m_patrolData.m_voxelMap.blockSize.y * 3, m_patrolData.m_voxelMap.blockSize.z);
		Vector3 offset = new Vector3 (upperBlock.x / 2, upperBlock.y / 2, upperBlock.z / 2);

		float x = logicPos.x * m_patrolData.m_voxelMap.blockSize.x + offset.x + parentPos.x;
		float y = parentPos.y;
		float z = -(logicPos.z * m_patrolData.m_voxelMap.blockSize.z + offset.z) + parentPos.z;

		GameObject go = GameObject.Instantiate (m_patrolData.m_referent, m_prop.transform);
		go.name = "x:" + logicPos.x + " y:" + logicPos.y + " z:" + logicPos.z;
		go.transform.position = new Vector3 (x, y, z);
		m_patrolData.m_patrolReferent.Add (go);

	}
}
