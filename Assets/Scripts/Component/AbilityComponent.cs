using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityComponent : BasicComponent
{
	public List<ComponentType> m_temporaryAbility = new List<ComponentType> ();
	public List<ComponentType> m_residentAbility = new List<ComponentType> ();
    public Dictionary<ComponentType, int> m_coldDown =new Dictionary<ComponentType, int>();
}
