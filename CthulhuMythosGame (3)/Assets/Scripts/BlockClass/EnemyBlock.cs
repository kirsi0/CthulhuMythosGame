using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBlock : BasicBlock
{

	public EnemyBlock (string aName)
	{
		m_blockType = BlockType.Enemy;
		m_name = aName;
	}


}
