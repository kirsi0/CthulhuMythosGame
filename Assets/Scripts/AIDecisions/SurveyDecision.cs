using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurveyDecision<T> : BasicDecision<T> where T : BasicEnemy
{
	int tmpHp;



	public SurveyDecision ()
	{
		SetDecesionType (AIComponent.Decision.Survey);
	}

	public override void Enter (T enemyEntity)
	{
		enemyEntity.m_entity.GetComponent<AIComponent> ().m_currentDecision = AIComponent.Decision.Survey;

		Debug.Log ("enter survey!");
		enemyEntity.m_events.AddEvent (AIComponent.Decision.Survey);
		DeadComponent deadComp = (DeadComponent)enemyEntity.m_entity.GetSpecicalComponent (ComponentType.Dead);
		tmpHp = deadComp.hp;
		Survey (enemyEntity.m_entity);
		return;
	}

	public override void Execte (T enemyEntity)
	{
		Debug.Log ("exec survey!");
		MonitorComponent monitorComp = (MonitorComponent)enemyEntity.m_entity.GetSpecicalComponent (ComponentType.Monitor);
		PropertyComponent propertyComp = (PropertyComponent)enemyEntity.m_entity.GetSpecicalComponent (ComponentType.Property);
		DeadComponent deadComp = (DeadComponent)enemyEntity.m_entity.GetSpecicalComponent (ComponentType.Dead);

		//发现敌人
		if (monitorComp.m_enemy.Count != 0) {

			//倾向攻击攻击
			if (deadComp.hp / propertyComp.HP > 0.2f) {

				enemyEntity.ChangeDecision (AIComponent.Decision.Attack);
				return;

			} else {
				//如果有同伴在旁边，那么就进行攻击
				if (AiToInput.ExistEnemy ()) {
					enemyEntity.ChangeDecision (AIComponent.Decision.CallSupport);
					return;
				}

			}
		}
		//如果还是听到声音
		if (monitorComp.m_voice.Count == 0) {


			//if (deadComp.hp / propertyComp.HP > 0.2f) {
			//float j = Random.value;
			//float surveyDemage = enemyEntity.m_events.DecisionExpectDemage (AIComponent.Decision.Survey);
			//float patrolDemage = enemyEntity.m_events.DecisionExpectDemage (AIComponent.Decision.Patrol);
			//Debug.Log ("surveyDemage" + surveyDemage + "patrolDemage" + patrolDemage);
			//if (j > surveyDemage / (surveyDemage + patrolDemage)) {
			//如果听到声音，生命充足，且根据历史，直接前去调查可以造成更多的伤害

			enemyEntity.ChangeDecision (AIComponent.Decision.Patrol);
			//		return;
			//	}
			//} else {
			//	float j = Random.value;
			//	float surveyInjure = enemyEntity.m_events.DecisionExpectInjure (AIComponent.Decision.Survey);
			//	float patrolInjeure = enemyEntity.m_events.DecisionExpectInjure (AIComponent.Decision.Patrol);
			//	if (j > patrolInjeure / (surveyInjure + patrolInjeure)) {
			//		//听到声音。生命不足，但是前去调查可以保存更多的生命
			//		enemyEntity.ChangeDecision (AIComponent.Decision.Patrol);
			//		return;
			//	}
			//}
		}

		Debug.Log ("run survey!");
		//计算受伤
		enemyEntity.m_events.AddInjure (tmpHp - deadComp.hp);
		tmpHp = deadComp.hp;

		Survey (enemyEntity.m_entity);
		return;
	}

	public void Survey (BasicEntity entity)
	{
		List<Vector3> voice = entity.GetComponent<MonitorComponent> ().m_voice;
		Vector3 mPos = entity.GetComponent<BlockInfoComponent> ().m_logicPosition;
		List<Vector3> newVoice = new List<Vector3> ();
		List<int> i = new List<int> ();
		for (int j = 0; j < voice.Count; j++) {
			Vector3 pos = voice [j];
			if (mPos != pos && FindPath.GetPathByStep (mPos, pos, 100) != Vector3.down) {
				newVoice.Add (pos);
			} else {
				i.Add (j);
			}
		}
		for (int j = 0; j < i.Count; j++) {
			voice.Remove (voice [i [j]]);
		}

		if (newVoice.Count != 0) {
			AiToInput.Move (entity, newVoice [newVoice.Count - 1]);
		}

	}

	public override void Exit (T enemyEntity)
	{
		Debug.Log ("exit survey!");
		DeadComponent deadComp = (DeadComponent)enemyEntity.m_entity.GetSpecicalComponent (ComponentType.Dead);
		enemyEntity.m_entity.GetComponent<MonitorComponent> ().m_voice.Clear ();

		//计算受伤
		enemyEntity.m_events.AddInjure (tmpHp - deadComp.hp);
		tmpHp = deadComp.hp;
	}
}
