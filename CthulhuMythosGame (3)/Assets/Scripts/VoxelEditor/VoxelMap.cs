using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//绘制网格，保存blcoks的实际引用，存储各种数据
public class VoxelMap : MonoBehaviour
{
	//地图尺寸，以方块为单位
	public Vector2 mapSize = new Vector2 (20, 10);
	//基础方块尺寸
	private Vector3 blockSize = new Vector3 (2, 2, 2);
	//基础方块
	public GameObject basicBlock;
	//存储block和位置的对应信息
	public GameObject voxelLowerBlocks;
	//存储上层的方块和位置的对应信息
	public GameObject voxelUpperBlocks;
	//储存所有会使用到的方块
	public List<BasicBlock> allBlocks = new List<BasicBlock> ();

	public int blockID = -1;

	public bool isUpperLayer = false;

	public bool displayUpperGizmo = true;

	public BasicBlock GetBlockBrush {
		get {
			if (blockID < 0 || blockID >= allBlocks.Count) {
				return null;
			}
			return allBlocks [blockID] as BasicBlock;
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

	public void SetBlockSize (float ablockSize)
	{
		blockSize = new Vector3 (ablockSize, ablockSize, ablockSize);
	}

	public Vector3 GetBlockSize ()
	{
		return blockSize;
	}

	void OnDrawGizmosSelected ()
	{
		Vector3 pos = transform.position;

		Gizmos.color = Color.gray;
		int row = 0;
		int maxColumns = (int)mapSize.x;
		int total = (int)mapSize.x * (int)mapSize.y;
		//单位方块
		Vector3 lowerBlock = new Vector3 (blockSize.x, blockSize.y, blockSize.z);
		Vector3 upperBlock = new Vector3 (blockSize.x, blockSize.y * 3, blockSize.z);

		Vector3 offset = new Vector3 (lowerBlock.x / 2, lowerBlock.y / 2, lowerBlock.z / 2);

		for (int i = 0; i < total; i++) {

			int column = i % maxColumns;
			float newX = (column * lowerBlock.x) + offset.x + pos.x;
			float newY = -offset.y;
			float newZ = -(row * lowerBlock.z) - offset.z + pos.z;

			//  上层的方块为标准方块的三倍
			if (displayUpperGizmo) {
				Gizmos.DrawWireCube (new Vector3 (newX, newY - 4 * newY, newZ), upperBlock);

			} else {
				Gizmos.DrawWireCube (new Vector3 (newX, newY, newZ), lowerBlock);

			}
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
