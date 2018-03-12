
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//使用voxelMap的数据，实现笔刷的绘制
[CustomEditor (typeof (VoxelMap))]
public class VoxelMapEditor : Editor
{
	//引用的地图数据
	public VoxelMap voxelMap;

	//绘制用的笔刷
	BlockBrush brush;
	//鼠标与画面接触的位置
	Vector3 mouseHit;

	//确定方块的类型
	BlockType blockType = BlockType.None;

	//确定方块的名字
	string blockName = "";

	//判断鼠标是否在地图尺寸上
	bool mouseOnMap {
		get {
			//鼠标的位置需要与方块的大小相除
			return mouseHit.x > 0 &&
					   mouseHit.x / voxelMap.GetBlockSize ().x < voxelMap.mapSize.x &&
					   mouseHit.z < 0 &&
					   mouseHit.z / voxelMap.GetBlockSize ().z > -voxelMap.mapSize.y;
		}
	}

	public override void OnInspectorGUI ()
	{


		//不需要base.OnInspectorGUI ();
		EditorGUILayout.BeginVertical ();

		bool oldUpperLayer = voxelMap.isUpperLayer;
		voxelMap.isUpperLayer = EditorGUILayout.Toggle ("is up layer:", voxelMap.isUpperLayer);
		if (oldUpperLayer != voxelMap.isUpperLayer) {
			//ChangeActiveLayer ();
		}

		EditorGUILayout.BeginHorizontal ();
		bool oldDispUpeerLayerGizmo = voxelMap.displayUpperGizmo;
		voxelMap.displayUpperGizmo = EditorGUILayout.Toggle ("Switch Gizmo Upper / Lower :", voxelMap.displayUpperGizmo);
		if (oldDispUpeerLayerGizmo != voxelMap.displayUpperGizmo) {
			//ChangeActiveLayer ();
		}

		EditorGUILayout.EndHorizontal ();

		//如果旧的地图尺寸和新的不同，则进行
		Vector2 oldSize = voxelMap.mapSize;
		voxelMap.mapSize = EditorGUILayout.Vector2Field ("Voxel Map Size", voxelMap.mapSize);
		if (oldSize != voxelMap.mapSize) {
			UpdateCalculations ();
		}

		//如果基准方块被改变则重新建立笔刷
		GameObject oldBasicBlock = voxelMap.basicBlock;
		voxelMap.basicBlock = (GameObject)EditorGUILayout.ObjectField ("Basic Object:", voxelMap.basicBlock, typeof (GameObject), false);
		if (oldBasicBlock != voxelMap.basicBlock) {
			UpdateCalculations ();
			voxelMap.blockID = 0;
			CreateBrush ();
		}

		//选择需要加入保存的方块类型
		blockType = (BlockType)EditorGUILayout.EnumPopup ("Block Type", blockType);
		//输入对于方块合适的名字
		blockName = (string)EditorGUILayout.TextField ("New Blcok Name", blockName);
		if (GUILayout.Button ("Add New Blcoksd")) {
			//必须命名
			if (blockName != "") {
				if (blockType != BlockType.None) {
					switch (blockType) {

					case BlockType.Floor:
						FloorBlock floorBlock = new FloorBlock (blockName);
						//为了防止反序列化时没有数据恢复，需要手动赋值一次
						floorBlock.BlockType = BlockType.Floor;
						voxelMap.allBlocks.Add (floorBlock);
						break;

					case BlockType.Wall:
						WallBlock wallBlock = new WallBlock (blockName);
						wallBlock.BlockType = BlockType.Wall;
						voxelMap.allBlocks.Add (wallBlock);
						break;

					case BlockType.Player:
						PlayerBlock playerBlock = new PlayerBlock (blockName);
						playerBlock.BlockType = BlockType.Player;
						voxelMap.allBlocks.Add (playerBlock);
						break;

					case BlockType.Enemy:
						EnemyBlock enemyBlock = new EnemyBlock (blockName);
						enemyBlock.BlockType = BlockType.Enemy;
						voxelMap.allBlocks.Add (enemyBlock);
						break;

					case BlockType.Door:
						DoorBlock doorBlock = new DoorBlock (blockName);
						doorBlock.BlockType = BlockType.Door;
						voxelMap.allBlocks.Add (doorBlock);
						break;
					}

					blockType = BlockType.None;
				}
			}

		}



		//保存所有的类型进入对应的数组
		//QuickSort (voxelMap.allBlocks, 0, voxelMap.allBlocks.Count);
		//List<BasicBlock> tmp = new List<BasicBlock> ();
		//tmp [1] = new FloorBlock ("hy");
		//tmp [0] = new 
		//QuickSorting.QuickSort ();

		//显示现在保存的方块类型
		for (int i = 0; i < voxelMap.allBlocks.Count; i++) {
			EditorGUILayout.BeginHorizontal ();
			voxelMap.allBlocks [i].m_gameobject = (GameObject)EditorGUILayout.ObjectField (voxelMap.allBlocks [i].m_name + " " + voxelMap.allBlocks [i].GetType (), voxelMap.allBlocks [i].m_gameobject, typeof (GameObject), false);
			//删除预设方块
			if (GUILayout.Button ("Delete")) {
				Debug.Log ("Delete" + i + voxelMap.allBlocks [i].m_name);
				voxelMap.allBlocks.Remove (voxelMap.allBlocks [i]);


			}
			EditorGUILayout.EndHorizontal ();
		}

		//refresh brush mesh to new brush,
		UpdateBrush (voxelMap.GetBlockBrush.m_gameobject.GetComponentInChildren<MeshFilter> (),
					 voxelMap.GetBlockBrush.m_gameobject.GetComponentInChildren<MeshRenderer> ());
		if (GUILayout.Button ("Clear Blocks")) {
			if (EditorUtility.DisplayDialog ("Clear map's blocks?", "Are you sure?", "Clear", "Do not clear")) {
				ClearMap ();
			}
		}

		EditorGUILayout.EndVertical ();
	}

	//当这个物体变成可用时，进行绘制前的初始化
	void OnEnable ()
	{
		//将选中的实例作为目标类型传递给变量
		voxelMap = target as VoxelMap;

		//如果没有用于保存所有准备绘制方块所需的父对象，则新建一个
		if (voxelMap.voxelLowerBlocks == null) {
			var go = new GameObject ("voxelLowerBlocks");
			go.transform.SetParent (voxelMap.transform);
			go.transform.position = Vector3.zero;

			//在脚本中保存所有的方块数据
			VoxelBlocks voxelBlocks = go.AddComponent<VoxelBlocks> ();
			voxelBlocks.MapSize = voxelMap.mapSize;
			voxelBlocks.BlockSize = voxelMap.GetBlockSize ();
			voxelMap.voxelLowerBlocks = go;
		}

		if (voxelMap.voxelUpperBlocks == null) {
			var go = new GameObject ("voxelUpperBlocks");
			go.transform.SetParent (voxelMap.transform);
			go.transform.position = Vector3.zero;

			VoxelBlocks voxelBlocks = go.AddComponent<VoxelBlocks> ();
			voxelBlocks.MapSize = voxelMap.mapSize;
			voxelBlocks.BlockSize = voxelMap.GetBlockSize ();
			voxelMap.voxelUpperBlocks = go;
		}

		//重新计算gizmo中方块的大小，并且新建一个笔刷
		if (voxelMap.basicBlock != null) {
			UpdateCalculations ();
			NewBrush ();
		}

	}

	//当这个物体变成不可用时，销毁笔刷
	void OnDisable ()
	{
		DestroyBrush ();
	}

	//当鼠标在Scene中时
	private void OnSceneGUI ()
	{

		if (brush != null) {
			//计算鼠标射线到地图平面的位置
			UpdateHitPosition ();

			//移动笔刷所在的位置
			MoveBrush ();

			//进行绘制或移除
			if (voxelMap.basicBlock != null && mouseOnMap) {
				Event current = Event.current;
				if (current.shift) {
					Draw ();
				}
				if (current.command || current.control) {
					RemoveBlock ();
				}
			}
		}
	}


	//重新计算方块的尺寸，以便重新绘制gizmo
	void UpdateCalculations ()
	{
		voxelMap.SetBlockSize (voxelMap.basicBlock.GetComponentInChildren<MeshFilter> ().sharedMesh.bounds.size.x);
	}

	void CreateBrush ()
	{
		//获得由picker选中的笔刷对象
		BasicBlock block = voxelMap.GetBlockBrush;

		if (block != null) {
			GameObject go = new GameObject ("Brush");
			go.transform.SetParent (voxelMap.transform);

			//笔刷中保存笔刷实例的引用
			brush = go.AddComponent<BlockBrush> ();
			brush.m_meshFilter = go.AddComponent<MeshFilter> ();
			brush.m_meshRender = go.AddComponent<MeshRenderer> ();

			brush.m_tag = block.m_tag;

			//使用brush的方法对笔刷实例的组件进行修改
			brush.m_brushSize = voxelMap.GetBlockSize ();
			brush.UpdateBrush (block.m_gameobject.GetComponentInChildren<MeshFilter> (),
							   block.m_gameobject.GetComponentInChildren<MeshRenderer> ());
		}
	}

	//新建笔刷
	void NewBrush ()
	{
		if (brush == null) {
			CreateBrush ();
		}
	}

	//删除笔刷
	void DestroyBrush ()
	{
		if (brush != null) {
			DestroyImmediate (brush.gameObject);
		}
	}

	//更新笔刷，用于picker中的所选元素发生改变的时候使用
	public void UpdateBrush (MeshFilter aMeshFilter, MeshRenderer aMeshRenderer)
	{
		if (brush != null) {
			brush.UpdateBrush (aMeshFilter, aMeshRenderer);

		}
	}

	//更新鼠标射线位置
	void UpdateHitPosition ()
	{
		//新建平面作为被射击的物体
		Plane p = new Plane (voxelMap.transform.TransformDirection (Vector3.up), Vector3.zero);
		//从鼠标所在的屏幕位置发射射线
		Ray ray = HandleUtility.GUIPointToWorldRay (Event.current.mousePosition);
		Vector3 hit = Vector3.zero;
		float dist = 0f;

		//记录鼠标射线起点到击中点的位置
		if (p.Raycast (ray, out dist)) {
			hit = ray.origin + ray.direction.normalized * dist;
		}

		//鼠标击中的位置需要重新转化为父对象的坐标，防止父对象的移动导致的错位
		mouseHit = voxelMap.transform.InverseTransformPoint (hit);
	}

	//移动笔刷所在的位置
	void MoveBrush ()
	{
		Vector3 blockSize = voxelMap.GetBlockSize ();

		//计算所在的自定方块大小单位的整数位置
		float x = Mathf.Floor (mouseHit.x / blockSize.x) * blockSize.x;
		float z = Mathf.Floor (mouseHit.z / blockSize.z) * blockSize.z;

		//计算逻辑上的位置
		float row = x / blockSize.x;
		float column = Mathf.Abs (z / blockSize.z) - 1;

		//如果鼠标不再屏幕上，直接返回
		if (!mouseOnMap)
			return;

		//根据逻辑位置计算ID
		int id = (int)((column * voxelMap.mapSize.x) + row);

		brush.m_blockID = id;
		brush.m_LogicPosition = new Vector3 (column, row, voxelMap.displayUpperGizmo ? 1 : 0);

		if (voxelMap.displayUpperGizmo) {
			//真正实例化的位置需要加上偏移
			x += voxelMap.transform.position.x + blockSize.x / 2;
			z += voxelMap.transform.position.z + blockSize.z / 2;

			//将位置修改到正确的位置
			brush.transform.position = new Vector3 (x, voxelMap.transform.position.y, z);

		} else {
			//真正实例化的位置需要加上偏移
			x += voxelMap.transform.position.x + blockSize.x / 2;
			z += voxelMap.transform.position.z + blockSize.z / 2;

			//将位置修改到正确的位置
			brush.transform.position = new Vector3 (x, voxelMap.transform.position.y - blockSize.y, z);
		}

	}

	//实例化方块
	void Draw ()
	{
		string id = brush.m_blockID.ToString ();

		//获取笔刷位置
		float posX = brush.transform.position.x;
		float posY = brush.transform.position.y;
		float poxZ = brush.transform.position.z;
		Debug.Log ("voxelMap.name: " + voxelMap.name);

		if (voxelMap.displayUpperGizmo) {

			GameObject block = GameObject.Find (voxelMap.name + "/voxelUpperBlocks/block_" + id);

			//如果找不到现有的实例，则新建一个方块
			if (block == null) {
				block = new GameObject ("block_" + id);
				block.transform.SetParent (voxelMap.voxelUpperBlocks.transform);
				block.transform.position = new Vector3 (posX, posY, poxZ);
				block.AddComponent<MeshFilter> ();
				block.AddComponent<MeshRenderer> ();
				block.AddComponent<BasicEntity> ();
				block.AddComponent<BlockInfoComponent> ();
				block.AddComponent<PropertyComponent> ();
				//block.AddComponent<AbilityComponent> ();
			}
			block.tag = brush.m_tag;
			//传入方块的网格数据
			BlockInfoComponent blockInfoComponent = block.GetComponent<BlockInfoComponent> ();
			PropertyComponent propertyComponent = block.GetComponent<PropertyComponent> ();
			//AbilityComponent abilityComponent = block.GetComponent<AbilityComponent> ();

			block.GetComponent<MeshFilter> ().sharedMesh = brush.m_meshFilter.sharedMesh;
			block.GetComponent<MeshRenderer> ().sharedMaterial = brush.m_meshRender.sharedMaterial;
			//
			blockInfoComponent.m_blockType = voxelMap.GetBlockBrush.BlockType;
			blockInfoComponent.m_blockName = voxelMap.GetBlockBrush.m_name;
			blockInfoComponent.m_logicPosition = brush.m_LogicPosition;
			blockInfoComponent.m_componentType = ComponentType.BlockInfo;
			blockInfoComponent.m_entity = block.GetComponent<BasicEntity> ();
			propertyComponent.m_componentType = ComponentType.Property;
			propertyComponent.m_entity = block.GetComponent<BasicEntity> ();
			//abilityComponent.m_componentType = ComponentType.Ability;
			//abilityComponent.m_entity = block.GetComponent<BasicEntity> ();

		} else {
			GameObject block = GameObject.Find (voxelMap.name + "/voxelLowerBlocks/block_" + id);

			//如果找不到现有的实例，则新建一个方块
			if (block == null) {
				block = new GameObject ("block_" + id);
				block.transform.SetParent (voxelMap.voxelLowerBlocks.transform);
				block.transform.position = new Vector3 (posX, posY, poxZ);
				block.AddComponent<MeshFilter> ();
				block.AddComponent<MeshRenderer> ();
				block.AddComponent<BasicEntity> ();
				block.AddComponent<BlockInfoComponent> ();
				block.AddComponent<PropertyComponent> ();
				//block.AddComponent<AbilityComponent> ();
			}
			block.tag = brush.m_tag;
			//传入方块的网格数据
			BlockInfoComponent blockInfoComponent = block.GetComponent<BlockInfoComponent> ();
			PropertyComponent propertyComponent = block.GetComponent<PropertyComponent> ();
			//AbilityComponent abilityComponent = block.GetComponent<AbilityComponent> ();
			block.GetComponent<MeshFilter> ().sharedMesh = brush.m_meshFilter.sharedMesh;
			block.GetComponent<MeshRenderer> ().sharedMaterial = brush.m_meshRender.sharedMaterial;
			//
			blockInfoComponent.m_blockType = voxelMap.GetBlockBrush.BlockType;
			blockInfoComponent.m_blockName = voxelMap.GetBlockBrush.m_name;
			blockInfoComponent.m_logicPosition = brush.m_LogicPosition;
			blockInfoComponent.m_componentType = ComponentType.BlockInfo;
			blockInfoComponent.m_entity = block.GetComponent<BasicEntity> ();
			propertyComponent.m_componentType = ComponentType.Property;
			propertyComponent.m_entity = block.GetComponent<BasicEntity> ();
			//abilityComponent.m_componentType = ComponentType.Ability;
			//abilityComponent.m_entity = block.GetComponent<BasicEntity> ();
		}


		//Debug.Log (voxelMap.name + "/voxelLowerBlocks");
		//GameObject voxelLowerBlocks = GameObject.Find (voxelMap.name + "/voxelLowerBlocks");
		//voxelLowerBlocks.GetComponent<VoxelBlocks> ().mapData.Add (new Vector3 (1, 1, 1), BlockType.Door);

	}

	//移除某一个方块
	void RemoveBlock ()
	{
		string id = brush.m_blockID.ToString ();
		if (voxelMap.displayUpperGizmo) {
			GameObject block = GameObject.Find (voxelMap.name + "/voxelUpperBlocks/block_" + id);
			if (block != null) {
				DestroyImmediate (block);
			}
		} else {
			GameObject block = GameObject.Find (voxelMap.name + "/voxelLowerBlocks/block_" + id);
			if (block != null) {
				DestroyImmediate (block);
			}
		}



	}

	//清空所有的方块
	void ClearMap ()
	{
		if (voxelMap.displayUpperGizmo) {
			for (int i = 0; i < voxelMap.voxelLowerBlocks.transform.childCount; i++) {
				Transform t = voxelMap.voxelLowerBlocks.transform.GetChild (i);
				DestroyImmediate (t.gameObject);
				i--;
			}
		} else {
			for (int i = 0; i < voxelMap.voxelUpperBlocks.transform.childCount; i++) {
				Transform t = voxelMap.voxelLowerBlocks.transform.GetChild (i);
				DestroyImmediate (t.gameObject);
				i--;
			}
		}

	}

	//void ChangeActiveLayer ()
	//{

	//}


}
