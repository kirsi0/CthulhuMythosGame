using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class DoorBlock : BasicBlock
{
	public DoorBlock (string aName)
	{
		m_blockType = BlockType.Door;
		m_name = aName;
	}

}
