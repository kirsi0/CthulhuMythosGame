using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PatrolDecision<T> : BasicDecision<T> where T : BasicEnemy
{
	public int m_tmpHp = 0;

	public PatrolDecision ()
	{
		SetDecesionType (AIComponent.Decision.Patrol);
	}

	public override void Enter (T enemyEntity)
	{
		enemyEntity.m_entity.GetComponent<AIComponent> ().m_currentDecision = AIComponent.Decision.Patrol;

		enemyEntity.m_events.AddEvent (AIComponent.Decision.Patrol);
		//记录回合开始时的生命值
		DeadComponent deadComp = (DeadComponent)enemyEntity.m_entity.GetSpecicalComponent (ComponentType.Dead);
		m_tmpHp = deadComp.hp;
		Debug.Log ("enter patrol!");
		Patrol (enemyEntity.m_entity);
		return;

	}


	public override void Execte (T enemyEntity)
	{
		Debug.Log ("check patrol!");
		MonitorComponent monitorComp = (MonitorComponent)enemyEntity.m_entity.GetSpecicalComponent (ComponentType.Monitor);
		PropertyComponent propertyComp = (PropertyComponent)enemyEntity.m_entity.GetSpecicalComponent (ComponentType.Property);
		DeadComponent deadComp = (DeadComponent)enemyEntity.m_entity.GetSpecicalComponent (ComponentType.Dead);


		if (monitorComp.m_enemy.Count != 0) {

			//if (deadComp.hp / propertyComp.HP > 0.2f) {

			//发现敌人，且生命值充足
			enemyEntity.ChangeDecision (AIComponent.Decision.Attack);
			return;

			//} else {

			//	if (AiToInput.ExistEnemy () == true) {
			//		enemyEntity.ChangeDecision (AIComponent.Decision.CallSupport);
			//		return;
			//	}

			//}
		}
		if (monitorComp.m_voice.Count != 0) {

			//if (deadComp.hp / propertyComp.HP > 0.2f) {

			enemyEntity.ChangeDecision (AIComponent.Decision.Survey);
            return;
			//}

		}
		Debug.Log ("run patrol!");
		//计算在上一回合中受到的伤害
		enemyEntity.m_events.AddInjure (m_tmpHp - deadComp.hp);
		m_tmpHp = deadComp.hp;

		Patrol (enemyEntity.m_entity);
		return;
	}


	public void Patrol (BasicEntity entity)
	{
		//寻找一个坐标进行来回移动
		AIComponent ai = entity.GetComponent<AIComponent> ();
		StateComponent state = entity.GetComponent<StateComponent> ();

		List<Vector3> path = ai.m_patrolPoint;
		Vector3 entityPos = entity.GetComponent<BlockInfoComponent> ().m_logicPosition;
		if (path [0] == entityPos) {
			Vector3 t = path [0];
			path.Remove (t);
			path.Add (t);
		}
		AiToInput.Move (entity, path [0]);
	}

	public override void Exit (T enemyEntity)
	{
		Debug.Log ("exit patrol!");
		DeadComponent deadComp = (DeadComponent)enemyEntity.m_entity.GetSpecicalComponent (ComponentType.Dead);
		//记录生命值
		enemyEntity.m_events.AddInjure (m_tmpHp - deadComp.hp);
		m_tmpHp = deadComp.hp;
	}

}
