using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

public static class NewVoxelMapEditorMenu
{

	[MenuItem ("GameObject/Voxel Map")]
	public static void CreateVoxelEditor ()
	{
		GameObject go = new GameObject ("Voxel Map");
		go.AddComponent<VoxelMap> ();
	}
}
