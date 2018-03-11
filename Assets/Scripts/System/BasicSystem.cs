using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BasicSystem
{
	public virtual ComponentType M_LinkedType { set; get; }

	public virtual List<BasicEntity> Fliter (ComponentType type)
	{
		//Debug.Log ("system get " + type + " component");
		List<BasicEntity> entities = ComponentManager.Instance.GetSpecialEntity (type);


		if (entities.Count == 0) {
			//Debug.Log ("there is no entity linked to this type");
			return null;
		}

		return entities;
	}

	public abstract void Execute (List<BasicEntity> entities);

	public virtual void Init (List<BasicEntity> entities)
	{

	}
}


