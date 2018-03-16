using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolData : MonoBehaviour
{


	//用与作为标志物的go
	public GameObject m_referent;

	//储存巡逻会经过的几个点
	public List<Vector3> m_patrolPoint;
	//储存对于标志物的引用
	public List<GameObject> m_patrolReferent;

	//储存了世界数据的voxelmap
	public VoxelMap m_voxelMap = null;




	// Use this for initialization
	void Start ()
	{
		Destroy (this);
	}

	// Update is called once per frame
	void Update ()
	{

	}
}
