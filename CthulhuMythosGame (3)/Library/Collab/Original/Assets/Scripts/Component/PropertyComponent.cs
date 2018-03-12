using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropertyComponent : BasicComponent
{

	public enum CharacterType
	{
		Null,
		Veteran,
		Hacker,
		Drone,
		Heretic,
		Deepone,
	}

	public CharacterType m_characterType = CharacterType.Null;
	public int m_agility;
	public int SPD;
	public float moveSpd;
	public int STR;
	public int HP;
	public int San;
	public int AP;
	public int HRZ;
    public int itemLimit;
    public List<ItemType> item;
    
	//TODO
	public List<Vector3> m_patrolPoint = new List<Vector3> {
					new Vector3 (6,1,1),
					new Vector3 (5,1,1),new Vector3 (4,2,1)
	};

    private void Start()
    {
        if (m_patrolPoint.Count != 0)
            return;
        m_patrolPoint.Add(m_entity.GetComponent<BlockInfoComponent>().m_logicPosition);
        
    }
}
