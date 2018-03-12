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

        foreach (BasicEntity e in entities)
        {
            AIComponent aiInfo = e.GetComponent<AIComponent>();
            if (aiInfo == null)
                continue;
            if (aiInfo.m_enemyState == EnemyState.Action)
            {
                //Debug.Log (StateStaticComponent.m_currentEntity + " aciont is going");
                AbilityManager abilityManager = new AbilityManager();
                abilityManager.RemoveTemppraryComponent(e);
                aiInfo.m_enemyState = EnemyState.Select;
            }
            //只有在系统结束工作之后才会重新开始执行
            if (aiInfo.m_enemyState == EnemyState.Wait)
            {
                StateComponent state = (StateComponent)e.GetSpecicalComponent(ComponentType.State);
                //增加输入组件
                AbilityManager abilityManager = new AbilityManager();
                //根据能力表新建新的组件
                abilityManager.AddTemporaryComponent(e);
                if (state.m_actionPoint > 0)
                {
                    //切换状态
                    aiInfo.m_enemyState = EnemyState.Select;
                    aiInfo.m_enemy.Execute();
                }
                else
                {
                    if (aiInfo.m_coldTime < aiInfo.m_actionInterval)
                    {
                        aiInfo.m_coldTime += Time.deltaTime;
                    }
                    else
                    {
                        aiInfo.m_coldTime = 0;
                        state.m_actionPoint = e.GetComponent<PropertyComponent>().AP;
                        aiInfo.m_enemyState = EnemyState.Select;
                        aiInfo.m_enemy.Execute();
                    }
                }
            }

            //if (turnNumber - StateStaticComponent.m_EturnNumber != 0)
            //{
            //    turnNumber++;
            //    if (StateStaticComponent.m_EcurrentEntity == e)
            //    {
            //        AIComponent aiComponent = (AIComponent)e.GetSpecicalComponent(ComponentType.AI);
            //        aiComponent.m_enemy.Execute();
            //    }
            //}
        }

	}
}
