using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof (VoxelCharacter))]
public class VoxelCharacterEditor : Editor
{

	//存储上层的角色信息
	public VoxelCharacter voxelCharacter;

	CharacterBrush brush;

	//确定方块的类型
	BlockType blockType = BlockType.None;

	//确定方块的名字
	string blockName = "";

	//鼠标与画面接触的位置
	Vector3 mouseHit;

	//判断鼠标是否在地图尺寸上
	bool mouseOnMap {
		get {
			//鼠标的位置需要与方块的大小相除
			return mouseHit.x > 0 &&
						   mouseHit.x / voxelCharacter.parentBlocks.GetBlockSize ().x < voxelCharacter.parentBlocks.mapSize.x &&
					   mouseHit.z < 0 &&
						   mouseHit.z / voxelCharacter.parentBlocks.GetBlockSize ().z > -voxelCharacter.parentBlocks.mapSize.y;
		}
	}

	public override void OnInspectorGUI ()
	{
		//被选择的方块标志，使用和obj模型不同的方式作为标示
		voxelCharacter.m_selectedbox = (GameObject)EditorGUILayout.ObjectField ("SelectedBox", voxelCharacter.m_selectedbox, typeof (GameObject), false);


		//选择需要加入保存的方块类型
		blockType = (BlockType)EditorGUILayout.EnumPopup ("Character Type", blockType);
		//输入对于方块合适的名字
		blockName = (string)EditorGUILayout.TextField ("New Character Name", blockName);
		if (GUILayout.Button ("Add New Character")) {
			//必须命名
			if (blockName != "") {
				if (blockType != BlockType.None) {
					switch (blockType) {

					case BlockType.Player:
						PlayerBlock playerBlock = new PlayerBlock (blockName);
						playerBlock.BlockType = BlockType.Player;
						voxelCharacter.allCharacter.Add (playerBlock);
						break;

					case BlockType.Enemy:
						EnemyBlock enemyBlock = new EnemyBlock (blockName);
						enemyBlock.BlockType = BlockType.Enemy;
						voxelCharacter.allCharacter.Add (enemyBlock);
						break;

					default:
						EditorUtility.DisplayDialog ("此类型不支持在该编辑器中编辑", "如果是非角色类型请在父对象方块编辑器中操作。", "确定");
						break;

					}

					blockType = BlockType.None;
				}
			}

		}


		//显示现在保存的方块类型
		for (int i = 0; i < voxelCharacter.allCharacter.Count; i++) {
			EditorGUILayout.BeginHorizontal ();
			voxelCharacter.allCharacter [i].m_gameobject = (GameObject)EditorGUILayout.ObjectField (voxelCharacter.allCharacter [i].m_name + " " + voxelCharacter.allCharacter [i].GetType (), voxelCharacter.allCharacter [i].m_gameobject, typeof (GameObject), false);
			//删除预设方块
			if (GUILayout.Button ("Delete")) {
				Debug.Log ("Delete" + i + voxelCharacter.allCharacter [i].m_name);
				voxelCharacter.allCharacter.Remove (voxelCharacter.allCharacter [i]);

			}
			EditorGUILayout.EndHorizontal ();
		}


	}

	private void OnEnable ()
	{
		voxelCharacter = target as VoxelCharacter;

		//重新计算gizmo中方块的大小，并且新建一个笔刷
		if (voxelCharacter.parentBlocks.basicBlock != null) {

			NewBrush ();
		}
	}

	private void OnSceneGUI ()
	{
		if (brush != null) {
			UpdateHitPosition ();

			//移动笔刷所在的位置
			MoveBrush ();


			//进行绘制或移除
			if (voxelCharacter.parentBlocks.basicBlock != null && mouseOnMap) {
				Event current = Event.current;
				if (current.Equals (Event.KeyboardEvent ("j"))) {
					Debug.Log ("asdaadsads");
					RotateModel (1);
				}

				if (current.shift) {
					Draw ();
				}
				if (current.command || current.control) {
					RemoveBlock ();
				}
			}
		}
	}

	//当这个物体变成不可用时，销毁笔刷
	void OnDisable ()
	{
		DestroyBrush ();
	}


	//移动笔刷所在的位置
	void MoveBrush ()
	{
		Vector3 blockSize = voxelCharacter.parentBlocks.GetBlockSize ();

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
		int id = (int)((column * voxelCharacter.parentBlocks.mapSize.x) + row);

		brush.m_blockID = id;
		brush.m_LogicPosition = new Vector3 (column, row, voxelCharacter.parentBlocks.displayUpperGizmo ? 1 : 0);


		//真正实例化的位置需要加上偏移
		x += voxelCharacter.parentBlocks.transform.position.x + blockSize.x / 2;
		z += voxelCharacter.parentBlocks.transform.position.z + blockSize.z / 2;

		//将位置修改到正确的位置
		brush.transform.position = new Vector3 (x, voxelCharacter.parentBlocks.transform.position.y, z);



	}
	void RotateModel (int direction)
	{
		brush.transform.Rotate (new Vector3 (0, 90 * direction, 0));
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


	void CreateBrush ()
	{
		//获得由picker选中的笔刷对象
		BasicBlock block = voxelCharacter.GetSelectedCharacter;

		if (block != null) {
			GameObject go = GameObject.Instantiate (voxelCharacter.m_selectedbox, voxelCharacter.transform);
			//go.transform.SetParent (voxelCharacter.transform);

			//笔刷中保存笔刷实例的引用
			brush = go.AddComponent<CharacterBrush> ();
			//brush.m_meshFilter = go.AddComponent<MeshFilter> ();
			//brush.m_meshRender = go.AddComponent<MeshRenderer> ();
			brush.m_gameObject = block.m_gameobject;
			brush.m_tag = block.m_tag;

			//使用brush的方法对笔刷实例的组件进行修改
			//brush.m_brushSize = voxelMap.GetBlockSize ();
			//brush.UpdatePosition (block.m_gameobject.GetComponentInChildren<MeshFilter> (),
			//block.m_gameobject.GetComponentInChildren<MeshRenderer> ());
		}
	}

	//更新鼠标射线位置
	void UpdateHitPosition ()
	{
		//新建平面作为被射击的物体
		Plane p = new Plane (voxelCharacter.parentBlocks.transform.TransformDirection (Vector3.up), Vector3.zero);
		//从鼠标所在的屏幕位置发射射线
		Ray ray = HandleUtility.GUIPointToWorldRay (Event.current.mousePosition);
		Vector3 hit = Vector3.zero;
		float dist = 0f;

		//记录鼠标射线起点到击中点的位置
		if (p.Raycast (ray, out dist)) {
			hit = ray.origin + ray.direction.normalized * dist;
		}

		//鼠标击中的位置需要重新转化为父对象的坐标，防止父对象的移动导致的错位
		mouseHit = voxelCharacter.parentBlocks.transform.InverseTransformPoint (hit);

	}

	//实例化方块
	void Draw ()
	{
		string id = brush.m_blockID.ToString ();

		//获取笔刷位置
		float posX = brush.transform.position.x;
		float posY = brush.transform.position.y;
		float poxZ = brush.transform.position.z;

		Quaternion rotate = brush.transform.localRotation;
		Debug.Log ("voxelMap.name: " + voxelCharacter.name);

		GameObject block = GameObject.Find (voxelCharacter.name + "/character_" + id);

		//如果找不到现有的实例，则新建一个方块
		if (block == null) {
			block = GameObject.Instantiate (brush.m_gameObject, voxelCharacter.transform);
			block.name = "character_" + id;
			block.transform.SetParent (voxelCharacter.transform);
			block.transform.position = new Vector3 (posX, posY, poxZ);
			block.transform.localRotation = rotate;

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

		//
		blockInfoComponent.m_blockType = voxelCharacter.GetSelectedCharacter.BlockType;
		blockInfoComponent.m_blockName = voxelCharacter.GetSelectedCharacter.m_name;
		blockInfoComponent.m_logicPosition = brush.m_LogicPosition;
		blockInfoComponent.m_componentType = ComponentType.BlockInfo;
		blockInfoComponent.m_entity = block.GetComponent<BasicEntity> ();
		propertyComponent.m_componentType = ComponentType.Property;
		propertyComponent.m_entity = block.GetComponent<BasicEntity> ();
		//abilityComponent.m_componentType = ComponentType.Ability;
		//abilityComponent.m_entity = block.GetComponent<BasicEntity> ();



		//Debug.Log (voxelMap.name + "/voxelLowerBlocks");
		//GameObject voxelLowerBlocks = GameObject.Find (voxelMap.name + "/voxelLowerBlocks");
		//voxelLowerBlocks.GetComponent<VoxelBlocks> ().mapData.Add (new Vector3 (1, 1, 1), BlockType.Door);

	}

	//移除某一个方块
	void RemoveBlock ()
	{
		string id = brush.m_blockID.ToString ();

		GameObject block = GameObject.Find (voxelCharacter.name + "/character_" + id);
		if (block != null) {
			DestroyImmediate (block);
		}




	}


}

public class CharacterBrush : MonoBehaviour
{

	public Vector3 m_brushSize = Vector3.zero;
	public int m_blockID = 0;

	public GameObject m_gameObject;

	public Vector3 m_LogicPosition;

	public string m_tag;

	//void OnDrawGizmosSelected ()
	//{
	//	Gizmos.color = Color.red;
	//	Gizmos.DrawWireCube (transform.position, brushSize );
	//}

	public void UpdatePosition (Vector3 pos)
	{
		//mesh还未赋值获取不到数据，使用sharedMesh
		m_LogicPosition = pos;
	}
}
