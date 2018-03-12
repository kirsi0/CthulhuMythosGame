using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CallSupportDecision<T> : BasicDecision<T> where T : BasicEnemy
{
	int tmpHp;

	public CallSupportDecision ()
	{
		SetDecesionType (AIComponent.Decision.CallSupport);
	}

	public override void Enter (T enemyEntity)
	{
		enemyEntity.m_entity.GetComponent<AIComponent> ().m_currentDecision = AIComponent.Decision.CallSupport;

		enemyEntity.m_events.AddEvent (AIComponent.Decision.CallSupport);
		DeadComponent deadComp = (DeadComponent)enemyEntity.m_entity.GetSpecicalComponent (ComponentType.Dead);
		tmpHp = deadComp.hp;
		CallSupport (enemyEntity);
		return;

	}

	public int CalculateDistance (BasicEntity entity)
	{
		MonitorComponent monitorComp = (MonitorComponent)entity.GetSpecicalComponent (ComponentType.Monitor);
		BasicEntity player = monitorComp.m_enemy [0];
		player.GetComponent<BlockInfoComponent> ().m_blockType = BlockType.None;
		List<Vector3> pathLsit = FindPath.GetPath (entity.GetComponent<BlockInfoComponent> ().m_logicPosition,
						  player.GetComponent<BlockInfoComponent> ().m_logicPosition);
		if (pathLsit != null) {
			Debug.Log ("cant find path!!!!!!!!!!!");
			return 0;
		} else {
			return pathLsit.Count;
		}
	}


	public override void Execte (T enemyEntity)
	{
		MonitorComponent monitorComp = (MonitorComponent)enemyEntity.m_entity.GetSpecicalComponent (ComponentType.Monitor);
		PropertyComponent propertyComp = (PropertyComponent)enemyEntity.m_entity.GetSpecicalComponent (ComponentType.Property);
		DeadComponent deadComp = (DeadComponent)enemyEntity.m_entity.GetSpecicalComponent (ComponentType.Dead);

		//如果没有敌人
		if (monitorComp.m_enemy.Count == 0) {
			enemyEntity.ChangeDecision (AIComponent.Decision.Patrol);
			return;
		} else {
			////如果遇到了敌人，且敌人的距离足够近
			//double threat = FuzzyLogic.DistanceThreat (CalculateDistance (enemyEntity.m_entity), propertyComp.HP);

			//if (threat > 0.4f) {
			enemyEntity.ChangeDecision (AIComponent.Decision.Attack);
			return;
			//	return;
			//} else {
			//	enemyEntity.ChangeDecision (AIComponent.Decision.Escape);
			//	return;
			//}
		}

		//计算受伤 
		enemyEntity.m_events.AddInjure (tmpHp - deadComp.hp);
		tmpHp = deadComp.hp;

		CallSupport (enemyEntity);
		return;
	}

	public void CallSupport (BasicEnemy enemy)
	{
		BasicEntity entity = enemy.m_entity;

		List<BasicEntity> enemyList = entity.GetComponent<MonitorComponent> ().m_enemy;

		AiToInput.CallFriend (entity, enemyList);
	}

	public override void Exit (T enemyEntity)
	{
		DeadComponent deadComp = (DeadComponent)enemyEntity.m_entity.GetSpecicalComponent (ComponentType.Dead);

		//计算受伤
		enemyEntity.m_events.AddInjure (tmpHp - deadComp.hp);
		tmpHp = deadComp.hp;
	}
}