using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AttackDecision<T> : BasicDecision<T> where T : BasicEnemy
{
	int tmpHp;
	int tmpTargetHp;
	BasicEntity target;

	public AttackDecision ()
	{
		SetDecesionType (AIComponent.Decision.Attack);
	}

	public override void Enter (T enemyEntity)
	{
		enemyEntity.m_entity.GetComponent<AIComponent> ().m_currentDecision = AIComponent.Decision.Attack;
		Debug.Log ("enter attack!");
		MonitorComponent monitorComp = (MonitorComponent)enemyEntity.m_entity.GetSpecicalComponent (ComponentType.Monitor);
		target = monitorComp.m_enemy [0];
		DeadComponent selfDeadComp = (DeadComponent)enemyEntity.m_entity.GetSpecicalComponent (ComponentType.Dead);
		DeadComponent targetDeadComp = (DeadComponent)target.GetSpecicalComponent (ComponentType.Dead);
		tmpHp = selfDeadComp.hp;
		tmpTargetHp = targetDeadComp.hp;
		//enemyEntity.m_events.AddEvent (AIComponent.Decision.Attack);
		Attack (enemyEntity.m_entity);

		return;

	}

	public override void Execte (T enemyEntity)
	{

		Debug.Log ("exec attack!");
		MonitorComponent monitorComp = (MonitorComponent)enemyEntity.m_entity.GetSpecicalComponent (ComponentType.Monitor);
		PropertyComponent propertyComp = (PropertyComponent)enemyEntity.m_entity.GetSpecicalComponent (ComponentType.Property);
		DeadComponent deadComp = (DeadComponent)enemyEntity.m_entity.GetSpecicalComponent (ComponentType.Dead);

		//生命值不足,且上一个动作不是呼叫朋友
		if (deadComp.hp < 0.1f * propertyComp.HP && enemyEntity.m_events.CurrentDecision () != AIComponent.Decision.CallSupport) {
			//同时有朋友存在
			if (AiToInput.ExistEnemy ()) {
				enemyEntity.ChangeDecision (AIComponent.Decision.CallSupport);
				return;
			}

		}

		//如果敌人已经死亡
		if (target == null || target.m_components.Count == 0) {

			//如果附近还有敌人
			if (monitorComp.m_enemy.Count != 0) {
				target = monitorComp.m_enemy [0];
			}

			if (monitorComp.m_enemy.Count != 0) {

				if (deadComp.hp / propertyComp.HP > 0.3f) {

					// 如果有玩家，且敌人的生命值较高的话，切换对象攻击，切换对象会在enter时进行
					enemyEntity.ChangeDecision (AIComponent.Decision.Attack);
					return;
				} else {

					//有玩家，但是生命值不高，可以逃跑

					if (AiToInput.ExistEnemy () && enemyEntity.m_events.CurrentDecision () != AIComponent.Decision.CallSupport) {
						enemyEntity.ChangeDecision (AIComponent.Decision.CallSupport);
						return;
					}
				}
			}

			if (monitorComp.m_voice.Count != 0) {
				//float i = Random.value;

				//double degreeAttack = FuzzyLogic.AttackBelong (deadComp.hp, propertyComp.HP);
				//double degreeProtect = FuzzyLogic.ProtectBelong (deadComp.hp, propertyComp.HP);

				if (deadComp.hp / propertyComp.HP > 0.2f) {
					//听到声音，生命充足
					//float j = Random.value;
					//float surveyDemage = enemyEntity.m_events.DecisionExpectDemage (AIComponent.Decision.Survey);
					//float patrolDemage = enemyEntity.m_events.DecisionExpectDemage (AIComponent.Decision.Patrol);
					//Debug.Log ("surveyDemage" + surveyDemage + "patrolDemage" + patrolDemage);
					//根据历史计算哪种行动造成的伤害更多
					//if (j >0.3f) {
					enemyEntity.ChangeDecision (AIComponent.Decision.Survey);
					return;
					//} else {
					//enemyEntity.ChangeDecision (AIComponent.Decision.Patrol);
					//return;
					//}
				} else {
					//float j = Random.value;
					//float surveyInjure = enemyEntity.m_events.DecisionExpectInjure (AIComponent.Decision.Survey);
					//float patrolInjeure = enemyEntity.m_events.DecisionExpectInjure (AIComponent.Decision.Patrol);
					//if (j > 0.3f) {
					if (AiToInput.ExistEnemy () && enemyEntity.m_events.CurrentDecision () != AIComponent.Decision.CallSupport) {
						enemyEntity.ChangeDecision (AIComponent.Decision.CallSupport);
						return;
					}

					//} else {
					//enemyEntity.ChangeDecision (AIComponent.Decision.Patrol);
					//return;
					//}
				}
			}

			//敌人死亡且旁边没有敌人，返回巡逻
			enemyEntity.ChangeDecision (AIComponent.Decision.Patrol);
			return;
		}
		Debug.Log ("run attack!");
		//计算受伤
		DeadComponent targetDeadComp = (DeadComponent)target.GetSpecicalComponent (ComponentType.Dead);

		enemyEntity.m_events.AddInjure (tmpHp - deadComp.hp);
		enemyEntity.m_events.AddDemage (tmpTargetHp - targetDeadComp.hp);
		tmpHp = deadComp.hp;
		tmpTargetHp = targetDeadComp.hp;
		Attack (enemyEntity.m_entity);
		return;
	}

	public void Attack (BasicEntity entity)
	{
		VoxelBlocks map = GameObject.Find ("Voxel Map").transform.GetComponent<VoxelBlocks> ();
		Vector3 ePos = entity.GetComponent<BlockInfoComponent> ().m_logicPosition;

		Vector3 tPos = target.GetComponent<BlockInfoComponent> ().m_logicPosition;
		if ((Mathf.Abs (ePos.x - tPos.x) <= 1.5f) && (Mathf.Abs (ePos.y - tPos.y) <= 1.5f)) {
			AiToInput.Attack (entity, tPos);
		} else {
			target.GetComponent<BlockInfoComponent> ().m_blockType = BlockType.None;
			List<Vector3> path = FindPath.GetPath (ePos, tPos);
			target.GetComponent<BlockInfoComponent> ().m_blockType = BlockType.Player;
			if (path != null) {
				path.Remove (tPos);
				tPos = path [path.Count - 1];
				AiToInput.Move (entity, tPos);
			} else {
				Debug.Log ("Fail to Attack");
			}
		}

	}

	public override void Exit (T enemyEntity)
	{
		Debug.Log ("quit attack!");
		DeadComponent selfDeadComp = (DeadComponent)enemyEntity.m_entity.GetSpecicalComponent (ComponentType.Dead);

		if (target == null || target.m_components.Count == 0) {
			//如果敌人死亡了，那么把最后记录的敌人生命全部加上

			enemyEntity.m_events.AddDemage (tmpTargetHp);
			tmpTargetHp = 0;

		} else {
			//如果敌人没有死亡，那么获取当前的生命加入事件伤害中
			DeadComponent targetDeadComp = (DeadComponent)target.GetSpecicalComponent (ComponentType.Dead);
			enemyEntity.m_events.AddDemage (tmpTargetHp - targetDeadComp.hp);
			tmpTargetHp = targetDeadComp.hp;

		}
		//唯一一个会记录攻击的区域
		enemyEntity.m_events.AddInjure (tmpHp - selfDeadComp.hp);
		tmpHp = selfDeadComp.hp;
	}
}
