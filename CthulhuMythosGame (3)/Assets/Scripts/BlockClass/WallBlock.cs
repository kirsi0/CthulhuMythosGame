using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class WallBlock : BasicBlock
{

	public WallBlock (string aName)
	{
		m_blockType = BlockType.Wall;
		m_name = aName;
	}
}
