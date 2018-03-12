using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class FloorBlock : BasicBlock
{

	public FloorBlock (string aName)
	{
		m_blockType = BlockType.Floor;
		m_name = aName;
	}

}
