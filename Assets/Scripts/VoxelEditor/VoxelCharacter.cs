using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoxelCharacter : MonoBehaviour
{

	//储存所有角色的方块信息
	public List<BasicBlock> allCharacter = new List<BasicBlock> ();

	//存储block和位置的对应信息
	public VoxelBlocks voxelLowerBlocks;
	//存储上层的方块和位置的对应信息
	public VoxelBlocks voxelUpperBlocks;
	//储存父对象的信息
	public VoxelMap parentBlocks;

	//作为暂时的选项框
	public GameObject m_selectedbox;

	//当前选中的角色序号
	public int characterID = -1;
	public BasicBlock GetSelectedCharacter {
		get {
			if (characterID < 0 || characterID >= allCharacter.Count) {
				return null;
			}
			return allCharacter [characterID] as BasicBlock;
		}
	}


	// Use this for initialization
	void Start ()
	{

	}

	// Update is called once per frame
	void Update ()
	{

	}


	void OnDrawGizmosSelected ()
	{
		Vector3 pos = parentBlocks.transform.position;

		Gizmos.color = Color.gray;
		int row = 0;
		int maxColumns = (int)parentBlocks.mapSize.x;
		int total = (int)parentBlocks.mapSize.x * (int)parentBlocks.mapSize.y;
		//单位方块
		Vector3 lowerBlock = new Vector3 (parentBlocks.blockSize.x, parentBlocks.blockSize.y, parentBlocks.blockSize.z);
		Vector3 upperBlock = new Vector3 (parentBlocks.blockSize.x, parentBlocks.blockSize.y * 3, parentBlocks.blockSize.z);

		Vector3 offset = new Vector3 (lowerBlock.x / 2, lowerBlock.y / 2, lowerBlock.z / 2);

		for (int i = 0; i < total; i++) {

			int column = i % maxColumns;
			float newX = (column * lowerBlock.x) + offset.x + pos.x;
			float newY = -offset.y;
			float newZ = -(row * lowerBlock.z) - offset.z + pos.z;

			//  上层的方块为标准方块的三倍
			//在角色编辑器中，所有的方块都处于上层 

			Gizmos.DrawWireCube (new Vector3 (newX, newY - 4 * newY, newZ), upperBlock);


			if (column == maxColumns - 1) {
				row++;
			}

		}

		//Gizmos.color = Color.white;
		//var centerX = pos.x + (gridSize.x / 2);
		//var centerY = pos.y - (gridSize.y / 2);

		//Gizmos.DrawWireCube (new Vector2 (centerX, centerY), gridSize);

	}
}

