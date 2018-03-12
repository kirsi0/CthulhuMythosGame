using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public enum BlockType
{
	None,
	Floor,
	Wall,
	Player,
	Enemy,
	Item,
	Key,
	Door
}

[System.Serializable]
public class BasicBlock
{

	public string m_name;
	public GameObject m_gameobject;
	public string m_tag = "Untagged";
	protected BlockType m_blockType;

	public virtual BlockType BlockType {
		get {
			return m_blockType;
		}
		set {
			m_blockType = value;
		}
	}
}
