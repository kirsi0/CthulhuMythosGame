using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockInfoComponent : BasicComponent
{
	//V3<logicX, logicY, logicZ:bool(1 is up,0 is down)>
	public Vector3 m_logicPosition;
	public BlockType m_blockType;
	public string m_blockName;

}
