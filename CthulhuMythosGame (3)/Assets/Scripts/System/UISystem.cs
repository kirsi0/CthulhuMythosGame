using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISystem : BasicSystem
{

	private ComponentType m_linkedType = ComponentType.UI;

	//覆盖父类的m_linkedType
	public override ComponentType M_LinkedType {
		set {
			m_linkedType = value;
		}
		get {
			return m_linkedType;
		}
	}
	public override void Execute (List<BasicEntity> entities)
	{
	}
}
