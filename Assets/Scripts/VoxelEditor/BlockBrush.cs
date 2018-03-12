using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockBrush : MonoBehaviour
{

	public Vector3 m_brushSize = Vector3.zero;
	public int m_blockID = 0;
	public MeshFilter m_meshFilter;
	public MeshRenderer m_meshRender;

	public Vector3 m_LogicPosition;

	public string m_tag;

	//void OnDrawGizmosSelected ()
	//{
	//	Gizmos.color = Color.red;
	//	Gizmos.DrawWireCube (transform.position, brushSize );
	//}

	public void UpdateBrush (MeshFilter aMeshFliter, MeshRenderer aMeshRender)
	{
		//mesh还未赋值获取不到数据，使用sharedMesh
		m_meshFilter.sharedMesh = aMeshFliter.sharedMesh;
		m_meshRender.sharedMaterial = aMeshRender.sharedMaterial;
	}
}
