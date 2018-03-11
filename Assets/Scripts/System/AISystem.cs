using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISystem : BasicSystem
{
	int turnNumber;
	private ComponentType m_linkedType = ComponentType.AI;

	//覆盖父类的m_linkedType
	public override ComponentType M_LinkedType {
		set {
			m_linkedType = value;
		}
		get {
			return m_linkedType;
		}
	}
	//AI是长期组件，但是每回合只执行一次
	public override void Init (List<BasicEntity> entities)
	{
		turnNumber = StateStaticComponent.m_turneNumber;
		base.Init (entities);
		foreach (BasicEntity e in entities) {
			PropertyComponent propertyComp = (PropertyComponent)e.GetSpecicalComponent (ComponentType.Property);
			if (propertyComp.m_characterType == PropertyComponent.CharacterType.Heretic) {
				AIComponent aiComp = (AIComponent)e.GetSpecicalComponent (ComponentType.AI);
				aiComp.m_enemy = new Heretic (e);
				aiComp.m_patrolPoint = propertyComp.m_patrolPoint;
				Heretic heretic = (Heretic)aiComp.m_enemy;
				heretic.m_events.AddEvent (AIComponent.Decision.Patrol);
				heretic.m_events.m_events [0].m_usingActionPoint = 0;
				PatrolDecision<Heretic> a = (PatrolDecision<Heretic>)heretic.m_decisions [AIComponent.Decision.Patrol];
				a.m_tmpHp = propertyComp.HP;


				continue;
			}
		}
	}

	public override void Execute (List<BasicEntity> entities)
	{
		if (turnNumber - StateStaticComponent.m_turneNumber != 0) {
			foreach (BasicEntity e in entities) {
				if (StateStaticComponent.m_currentEntity == e) {
					AIComponent aiComponent = (AIComponent)e.GetSpecicalComponent (ComponentType.AI);
					aiComponent.m_enemy.Execute ();
				}
			}
			turnNumber++;
		}


	}
}
