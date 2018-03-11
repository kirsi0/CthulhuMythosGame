using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlock : BasicBlock
{
	public PlayerBlock (string aName)
	{
		m_blockType = BlockType.Door;
		m_name = aName;
	}


}
