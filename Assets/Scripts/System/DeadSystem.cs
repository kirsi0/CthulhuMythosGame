using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadSystem : BasicSystem
{

	private ComponentType m_linkedType = ComponentType.Dead;

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
		for (int i = 0; i < entities.Count; i++) {
			BasicEntity e = entities [i];
			DeadComponent dead = e.GetComponent<DeadComponent> ();
			PropertyComponent propertyComp = (PropertyComponent)e.GetSpecicalComponent (ComponentType.Property);

			if (dead.hp <= 0) {

				if (propertyComp.m_characterType == PropertyComponent.CharacterType.Heretic ||
				   propertyComp.m_characterType == PropertyComponent.CharacterType.Deepone) {
					AIComponent aiComp = (AIComponent)e.GetSpecicalComponent (ComponentType.AI);
					//todo
					//aiComp.m_enemy.Dead ();
				}
				//播放死亡动画
				RemoveDead (dead);
				dead.m_entity.RemoveAllComponent ();
				continue;
			}
			if (dead.san == 0) {

			}
		}
	}
	void RemoveDead (DeadComponent dead)
	{


		GameObject.Destroy (dead.gameObject, 1);
		//播放死亡动画
		VoxelBlocks map = GameObject.Find ("Voxel Map").transform.GetComponent<VoxelBlocks> ();
		map.RemoveBlock (dead.GetComponent<BlockInfoComponent> ().m_logicPosition);

	}
}
