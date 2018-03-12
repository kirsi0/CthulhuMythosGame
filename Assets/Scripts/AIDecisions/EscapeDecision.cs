using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EscapeDecision<T> : BasicDecision<T> where T : BasicEnemy
{
	int tmpHp;

	public EscapeDecision ()
	{
		//SetDecesionType (AIComponent.Decision.Escape);
	}
	public override void Enter (T enemyEntity)
	{

		//enemyEntity.m_events.AddEvent (AIComponent.Decision.Escape);
		DeadComponent deadComp = (DeadComponent)enemyEntity.m_entity.GetSpecicalComponent (ComponentType.Dead);
		tmpHp = deadComp.hp;
		Escape (enemyEntity);
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
		DeadComponent deadComp = (DeadComponent)enemyEntity.m_entity.GetSpecicalComponent (ComponentType.Dead);
		PropertyComponent propertyComponent = (PropertyComponent)enemyEntity.m_entity.GetSpecicalComponent (ComponentType.Property);

		//如果没有敌人
		if (monitorComp.m_enemy.Count == 0) {
			enemyEntity.ChangeDecision (AIComponent.Decision.Patrol);
			return;
		} else {
			//如果遇到了敌人，且敌人的距离足够近
			double threat = FuzzyLogic.DistanceThreat (CalculateDistance (enemyEntity.m_entity), propertyComponent.HP);

			if (threat > 0.4f) {
				enemyEntity.ChangeDecision (AIComponent.Decision.Attack);
				return;
			}
		}

		//当队友出现在视野里
		if (AiToInput.FriendInSight (enemyEntity.m_entity)) {
			enemyEntity.ChangeDecision (AIComponent.Decision.CallSupport);
			return;
		}

		//计算受伤
		enemyEntity.m_events.AddInjure (tmpHp - deadComp.hp);
		tmpHp = deadComp.hp;

		Escape (enemyEntity);
		return;
	}

	public void Escape (BasicEnemy enemy)
	{
		BasicEntity entity = enemy.m_entity;
		VoxelBlocks map = GameObject.Find ("Voxel Map").GetComponent<VoxelBlocks> ();

		List<BasicEntity> enemyList = entity.GetComponent<MonitorComponent> ().m_enemy;

		if (AiToInput.CallFriend (entity, enemyList)) {
			//呼叫成功
			StateComponent state = entity.GetComponent<StateComponent> ();
            state.AnimationStart();
			state.m_actionPoint -= 1;
			state.Invoke ("AnimationEnd", 1);
			return;
		}

		Vector3 escapePos = FindPath.GetNearestFriend (entity.GetComponent<BlockInfoComponent> ().m_logicPosition);
		if (escapePos == Vector3.down) {
			//无路可走等死
			return;
		} else {
			AiToInput.Move (entity, escapePos);
		}
		return;
	}

	public override void Exit (T enemyEntity)
	{
		DeadComponent deadComp = (DeadComponent)enemyEntity.m_entity.GetSpecicalComponent (ComponentType.Dead);
		enemyEntity.m_events.AddDemage (tmpHp - deadComp.hp);
		tmpHp = deadComp.hp;
	}
}
