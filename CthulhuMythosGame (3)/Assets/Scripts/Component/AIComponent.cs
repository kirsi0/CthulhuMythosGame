using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Skyunion;
//当还没有统计到足够数值的时候作为初始数值进行ai的判断
public static class Probability
{
	//静态自身受伤概率，概率越高，敌人越倾向于不攻击
	public static Dictionary<AIComponent.Decision, float> StaticBayesInjure = new Dictionary<AIComponent.Decision, float> (){
		{AIComponent.Decision.CallSupport, 0.5f},
		{AIComponent.Decision.Escape, 0.5f},
		{AIComponent.Decision.Patrol, 0.5f},
		{AIComponent.Decision.Survey, 0.5f}
	};
	//静态攻击伤害概率，概率越高，敌人攻击欲望越强
	public static Dictionary<AIComponent.Decision, float> StaticBayesDemage = new Dictionary<AIComponent.Decision, float> (){

		{AIComponent.Decision.CallSupport, 0.5f},
		{AIComponent.Decision.Escape, 0.5f},
		{AIComponent.Decision.Patrol, 0.5f},
		{AIComponent.Decision.Survey, 0.5f}
	};

	//更新静态数据
	public static void UpdateStatic (AIComponent.Decision decision, float newInjure, float newDem)
	{
		float tmpInjure = StaticBayesInjure [decision] + newInjure;
		float tmpDem = StaticBayesDemage [decision] + newDem;
		StaticBayesInjure [decision] = tmpInjure / 2;
		StaticBayesDemage [decision] = tmpDem / 2;
	}

}
public class AIComponent : BasicComponent
{
	public enum Decision
	{
		Null,
		Patrol,     //巡逻
		Survey,     //调查
		Attack,     //攻击
		Escape,     //逃跑
		CallSupport,//呼叫支援
	}
	//ai的核心
	public BasicEnemy enemy;

	public List<Vector3> m_patrolPoint = new List<Vector3> ();
}

//敌人基类
public abstract class BasicEnemy
{   //实体链接
	public BasicEntity m_entity;
	//敌人类型
	public PropertyComponent.CharacterType m_enemeyTyep;
	//历史事件
	public DecisionEvent m_events;
	//需要将实体传入
	public BasicEnemy (BasicEntity entity)
	{
		m_entity = entity;
		m_events = new DecisionEvent ();
	}
	public abstract void Execute ();
	public abstract void ChangeDecision (AIComponent.Decision newDecision);
	public abstract void Dead ();
}
//邪教徒 
public class Heretic : BasicEnemy
{
	//public int STR;
	//public int HP;
	//public int San;
	//public int AP;
	//public int HRZ;

	//当前所处的决策
	public BasicDecision<Heretic> m_currentDecision;
	//之前所处的决策
	public BasicDecision<Heretic> m_previsousDecsion;
	//实际的决策实体所在的位置
	public Dictionary<AIComponent.Decision, BasicDecision<Heretic>> m_decisions;
	//这个实体所用的类型
	AIComponent.Decision [] containDecision = {AIComponent.Decision.Patrol,
											   AIComponent.Decision.Survey,
											   AIComponent.Decision.Attack,
											   AIComponent.Decision.Escape,
											   AIComponent.Decision.CallSupport};

	public Heretic (BasicEntity entity) : base (entity)
	{

		m_enemeyTyep = PropertyComponent.CharacterType.Heretic;

		m_decisions = new Dictionary<AIComponent.Decision, BasicDecision<Heretic>> ();
		//根据枚举获取对应实例
		foreach (AIComponent.Decision decision in containDecision) {
			m_decisions.Add (decision, CreateDecision (decision));
		}
		//默认状态是巡逻状态
		m_currentDecision = m_decisions [AIComponent.Decision.Patrol];
		m_previsousDecsion = m_currentDecision;
	}

	public override void Dead ()
	{
		//在角色死亡的时候，把数据都更新到全局静态中
		//todo：当角色死亡时候，还需要做数据的整理
		foreach (AIComponent.Decision d in m_decisions.Keys) {
			Probability.UpdateStatic (d, m_events.BayesInjure (d), m_events.BayesDemage (d));
		}
	}
	//执行函数，每回合开始都是执行当前状态中的execute函数
	public override void Execute ()
	{
		m_currentDecision.Execte (this);
	}
	//改变当前状态
	public override void ChangeDecision (AIComponent.Decision newDecision)
	{
		m_previsousDecsion = m_currentDecision;
		m_previsousDecsion.Exit (this);
		m_currentDecision = m_decisions [newDecision];
		m_currentDecision.Enter (this);
	}
	//生产对应状态
	public BasicDecision<Heretic> CreateDecision (AIComponent.Decision decision)
	{
		switch (decision) {
		case AIComponent.Decision.Attack:
			return new AttackDecision<Heretic> ();

		case AIComponent.Decision.CallSupport:
			return new CallSupportDecision<Heretic> ();

		case AIComponent.Decision.Escape:
			return new EscapeDecision<Heretic> ();

		case AIComponent.Decision.Patrol:
			return new PatrolDecision<Heretic> ();

		case AIComponent.Decision.Survey:
			return new SurveyDecision<Heretic> ();

		default:
			return null;
		}

	}
}

public class DecisionEvent
{
	//这次状态切换开始到切换为下一个状态
	public class Event
	{
		public AIComponent.Decision m_decision;
		public int m_injure;
		public int m_demage;

		public int m_usingActionPoint;
		//默认为null状态
		public Event ()
		{
			m_decision = AIComponent.Decision.Null;
			m_injure = 0;
			m_demage = 0;
			m_usingActionPoint = 0;
		}
		public Event (AIComponent.Decision decision)
		{
			m_decision = decision;
			m_injure = 0;
			m_demage = 0;
			m_usingActionPoint = 0;

		}
	}
	//事件记录的长度
	const int eventLength = 40;

	//存储之前所有的事情
	public Event [] m_events;
	int m_currentEvent;

	public DecisionEvent ()
	{
		m_events = new Event [eventLength];
		m_currentEvent = 0;
	}
	//不把attack添加入决策事件中，而是加入上一个事件中
	public void AddEvent (AIComponent.Decision decision)
	{
		if (decision == AIComponent.Decision.Attack) {
			Debug.Log ("Add Error Decision");
			return;
		}
		Event e = new Event (decision);
		m_events [m_currentEvent] = e;
		m_events [m_currentEvent].m_usingActionPoint++;
		m_currentEvent++;
		if (m_currentEvent == eventLength) {
			m_currentEvent = 0;
		}
	}

	public void AddDemage (int demage)
	{
		m_events [m_currentEvent - 1].m_demage += demage;
		m_events [m_currentEvent - 1].m_usingActionPoint++;

	}

	public void AddInjure (int injure)
	{
		m_events [m_currentEvent - 1].m_injure += injure;
		m_events [m_currentEvent - 1].m_usingActionPoint++;

	}
	//状态的伤害期待 = 平均伤害数值* 造成伤害的可能性
	public float DecisionExpectDemage (AIComponent.Decision decision)
	{
		Debug.Log ("BayesInjure (decision)" + BayesInjure (decision));
		Debug.Log (" ExpectInjure (decision)" + ExpectInjure (decision));
		return BayesDemage (decision) * ExpectDemage (decision);
	}
	//状态的平均伤害数值
	public float ExpectDemage (AIComponent.Decision decision)
	{
		int demageTime = 0;
		float expectDemage = 0;
		foreach (Event e in m_events) {
			if (e == null) {
				continue;
			}
			if (e.m_decision == decision) {
				expectDemage += e.m_demage;
				demageTime += e.m_usingActionPoint;
			}
		}
		if (demageTime == 0) {
			return 1;
		}
		return expectDemage / demageTime;
	}

	//受到伤害的是由于前去执行一个行动的可能性 = 
	//执行这个行动的可能性*执行这个行动并且受伤的可能性／受到伤害的可能性
	//= 由于执行这个行动受到的伤害次数／总共的伤害次数
	public float BayesDemage (AIComponent.Decision decision)
	{
		//所有的行动
		int allExcute = 0;
		//对应行动的次数
		int decisionExecute = 0;
		//对应行动造成伤害的次数
		int executeDemage = 0;
		//总的进攻次数
		int allDemage = 0;
		foreach (Event e in m_events) {
			if (e == null) {
				continue;
			}
			if (e.m_demage != 0) {
				allDemage++;
			}
			if (e.m_decision != AIComponent.Decision.Null) {
				allExcute++;
				if (e.m_decision == decision) {
					decisionExecute++;
					if (e.m_demage != 0) {
						executeDemage++;
					}
				}
			}

		}
		if (allExcute == 0 || allDemage == 0 || decisionExecute == 0) {
			return Probability.StaticBayesDemage [decision];
		}
		float denominator = allDemage / allExcute;
		return ((executeDemage / decisionExecute) * (decisionExecute / allExcute)) / denominator;
	}
	//状态的受伤期待
	public float DecisionExpectInjure (AIComponent.Decision decision)
	{
		Debug.Log ("BayesInjure (decision)" + BayesInjure (decision));
		Debug.Log (" ExpectInjure (decision)" + ExpectInjure (decision));
		return BayesInjure (decision) * ExpectInjure (decision);
	}

	public float ExpectInjure (AIComponent.Decision decision)
	{
		int ingureTime = 0;
		float expectInjure = 0;
		foreach (Event e in m_events) {
			if (e == null) {
				continue;
			}
			if (e.m_decision == decision) {
				expectInjure += e.m_injure;
				ingureTime += e.m_usingActionPoint;
			}
		}
		if (ingureTime == 0) {
			return 1;
		}
		return expectInjure / ingureTime;
	}

	public float BayesInjure (AIComponent.Decision decision)
	{
		//所有的行动
		int allExcute = 0;
		//对应行动的次数
		int decisionExecute = 0;
		//对应行动造成伤害的次数
		int executeInjure = 0;
		//总的进攻次数
		int allInjure = 0;
		foreach (Event e in m_events) {
			if (e == null) {
				continue;
			}
			if (e.m_injure != 0) {
				allInjure++;
			}
			if (e.m_decision != AIComponent.Decision.Null) {
				allExcute++;
				if (e.m_decision == decision) {
					decisionExecute++;
					if (e.m_injure != 0) {
						executeInjure++;
					}
				}
			}

		}
		if (allExcute == 0 || allInjure == 0 || decisionExecute == 0) {
			return Probability.StaticBayesInjure [decision];
		}
		float denominator = allInjure / allExcute;
		return ((executeInjure / decisionExecute) * (decisionExecute / allExcute)) / denominator;
	}
}

public abstract class BasicDecision<T> where T : BasicEnemy
{
	public const int MAXMEMORY = 30;

	public AIComponent.Decision m_DecisionType;

	public void SetDecesionType (AIComponent.Decision type)
	{
		m_DecisionType = type;
	}

	public AIComponent.Decision GetDecesionType ()
	{
		return m_DecisionType;
	}

	public abstract void Enter (T enemyEntity);
	public abstract void Execte (T enemyEntity);
	public abstract void Exit (T enemyEntity);
}



public class PatrolDecision<T> : BasicDecision<T> where T : BasicEnemy
{
	public int m_tmpHp = 0;

	public PatrolDecision ()
	{
		SetDecesionType (AIComponent.Decision.Patrol);
	}

	public override void Enter (T enemyEntity)
	{
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
			//贝叶斯算法，确定损耗之后再做决定

			float i = Random.value;
			double degreeAttack = FuzzyLogic.AttackBelong (deadComp.hp, propertyComp.HP);
			double degreeProtect = FuzzyLogic.ProtectBelong (deadComp.hp, propertyComp.HP);
			if (i >= degreeProtect / (degreeAttack + degreeProtect)) {
				//Debug.Log ("yiyiyiiyiyiyiyiyiyi");
				//float j = Random.value;
				//float attackDemage = enemyEntity.m_events.DecisionExpectDemage (AIComponent.Decision.Attack);
				//float patrolDemage = enemyEntity.m_events.DecisionExpectDemage (AIComponent.Decision.Patrol);
				//if (j > patrolDemage / (attackDemage + patrolDemage)) {
				//发现敌人，且生命值充足
				enemyEntity.ChangeDecision (AIComponent.Decision.Attack);
				return;
				//}
			} else {
				//todo:call fridend

				//float j = Random.value;
				//float escapeInjure = enemyEntity.m_events.DecisionExpectInjure (AIComponent.Decision.Escape);
				//float patrolInjeure = enemyEntity.m_events.DecisionExpectInjure (AIComponent.Decision.Patrol);
				//if (j > (escapeInjure / (escapeInjure + patrolInjeure))) {
				//	enemyEntity.ChangeDecision (AIComponent.Decision.Attack);
				//	return;
				//}

				//生命不足，没有队友，选择逃跑
				enemyEntity.ChangeDecision (AIComponent.Decision.Escape);
			}
		}
		if (monitorComp.m_voice.Count != 0) {
			float i = Random.value;

			double degreeAttack = FuzzyLogic.AttackBelong (deadComp.hp, propertyComp.HP);
			double degreeProtect = FuzzyLogic.ProtectBelong (deadComp.hp, propertyComp.HP);

			if (i >= degreeProtect / (degreeAttack + degreeProtect)) {
				float j = Random.value;
				float surveyDemage = enemyEntity.m_events.DecisionExpectDemage (AIComponent.Decision.Survey);
				float patrolDemage = enemyEntity.m_events.DecisionExpectDemage (AIComponent.Decision.Patrol);
				Debug.Log ("surveyDemage" + surveyDemage + "patrolDemage" + patrolDemage);
				if (j > patrolDemage / (surveyDemage + patrolDemage)) {
					//如果听到声音，生命充足，且根据历史，直接前去调查可以造成更多的伤害
					enemyEntity.ChangeDecision (AIComponent.Decision.Survey);
					return;
				}
			} else {
				float j = Random.value;
				float surveyInjure = enemyEntity.m_events.DecisionExpectInjure (AIComponent.Decision.Survey);
				float patrolInjeure = enemyEntity.m_events.DecisionExpectInjure (AIComponent.Decision.Patrol);
				if (j > surveyInjure / (surveyInjure + patrolInjeure)) {
					//听到声音。生命不足，但是前去调查可以保存更多的生命
					enemyEntity.ChangeDecision (AIComponent.Decision.Survey);
					return;
				}
			}
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

		enemyEntity.m_events.AddInjure (m_tmpHp - deadComp.hp);
		m_tmpHp = deadComp.hp;
	}

}

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

		//生命值不足，必然进入逃跑
		if (deadComp.hp < 0.1f * propertyComp.HP) {
			enemyEntity.ChangeDecision (AIComponent.Decision.Escape);
			return;
		}

		//如果敌人已经死亡
		if (target == null || target.m_components.Count == 0) {

			////如果附近还有敌人
			//if (monitorComp.m_enemy.Count != 0) {
			//	target = monitorComp.m_enemy [0];
			//}

			if (monitorComp.m_enemy.Count != 0) {
				float i = Random.value;
				double degreeAttack = FuzzyLogic.AttackBelong (deadComp.hp, propertyComp.HP);
				double degreeProtect = FuzzyLogic.ProtectBelong (deadComp.hp, propertyComp.HP);
				if (i > degreeProtect / (degreeAttack + degreeProtect)) {
					//float j = Random.value;
					//float surveyDemage = enemyEntity.m_events.DecisionExpectDemage (AIComponent.Decision.Survey);
					//float patrolDemage = enemyEntity.m_events.DecisionExpectDemage (AIComponent.Decision.Patrol);
					//if (j > patrolDemage / (surveyDemage + patrolDemage)) {
					//	enemyEntity.ChangeDecision (AIComponent.Decision.Survey);
					//	return;
					//}
					// 如果有玩家，且敌人的生命值较高的话，切换对象攻击，切换对象会在enter时进行
					enemyEntity.ChangeDecision (AIComponent.Decision.Attack);
				} else {
					//float j = Random.value;
					//float surveyInjure = enemyEntity.m_events.DecisionExpectInjure (AIComponent.Decision.Survey);
					//float patrolInjeure = enemyEntity.m_events.DecisionExpectInjure (AIComponent.Decision.Patrol);
					//if (j > surveyInjure / (patrolInjeure + surveyInjure)) {
					//	enemyEntity.ChangeDecision (AIComponent.Decision.Survey);
					//	return;
					//}
					//有玩家，但是生命值不高，可以逃跑
					enemyEntity.ChangeDecision (AIComponent.Decision.Escape);
				}
			}

			if (monitorComp.m_voice.Count != 0) {
				float i = Random.value;

				double degreeAttack = FuzzyLogic.AttackBelong (deadComp.hp, propertyComp.HP);
				double degreeProtect = FuzzyLogic.ProtectBelong (deadComp.hp, propertyComp.HP);

				if (i >= degreeProtect / (degreeAttack + degreeProtect)) {
					//听到声音，生命充足
					float j = Random.value;
					float surveyDemage = enemyEntity.m_events.DecisionExpectDemage (AIComponent.Decision.Survey);
					float patrolDemage = enemyEntity.m_events.DecisionExpectDemage (AIComponent.Decision.Patrol);
					Debug.Log ("surveyDemage" + surveyDemage + "patrolDemage" + patrolDemage);
					//根据历史计算哪种行动造成的伤害更多
					if (j > patrolDemage / (surveyDemage + patrolDemage)) {
						enemyEntity.ChangeDecision (AIComponent.Decision.Survey);
						return;
					} else {
						enemyEntity.ChangeDecision (AIComponent.Decision.Patrol);
						return;
					}
				} else {
					float j = Random.value;
					float surveyInjure = enemyEntity.m_events.DecisionExpectInjure (AIComponent.Decision.Survey);
					float patrolInjeure = enemyEntity.m_events.DecisionExpectInjure (AIComponent.Decision.Patrol);
					if (j > surveyInjure / (surveyInjure + patrolInjeure)) {
						enemyEntity.ChangeDecision (AIComponent.Decision.Survey);
						return;
					} else {
						enemyEntity.ChangeDecision (AIComponent.Decision.Patrol);
						return;
					}
				}
			}

			//返回巡逻
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

		enemyEntity.m_events.AddInjure (tmpHp - selfDeadComp.hp);
		tmpHp = selfDeadComp.hp;
	}
}

public class EscapeDecision<T> : BasicDecision<T> where T : BasicEnemy
{
	int tmpHp;

	public EscapeDecision ()
	{
		SetDecesionType (AIComponent.Decision.Escape);
	}
	public override void Enter (T enemyEntity)
	{

		enemyEntity.m_events.AddEvent (AIComponent.Decision.Escape);
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
			StateStaticComponent.m_currentSystemState = StateStaticComponent.SystemState.Action;
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

public class SurveyDecision<T> : BasicDecision<T> where T : BasicEnemy
{
	int tmpHp;

	public SurveyDecision ()
	{
		SetDecesionType (AIComponent.Decision.Survey);
	}

	public override void Enter (T enemyEntity)
	{
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
			//贝叶斯算法，确定损耗之后再做决定
			//
			double degreeAttack = FuzzyLogic.AttackBelong (deadComp.hp, propertyComp.HP);
			double degreeProtect = FuzzyLogic.ProtectBelong (deadComp.hp, propertyComp.HP);
			//倾向攻击攻击
			if (degreeAttack >= degreeProtect) {
				//float attackDemage = enemyEntity.m_events.DecisionExpectDemage (AIComponent.Decision.Attack);
				//float patrolDemage = enemyEntity.m_events.DecisionExpectDemage (AIComponent.Decision.Patrol);
				//if (attackDemage > patrolDemage) {
				enemyEntity.ChangeDecision (AIComponent.Decision.Attack);
				return;
				//} else {
				//	if(AiToInput.FriendInSight (enemyEntity.m_entity) == true){
				//		enemyEntity.ChangeDecision (AIComponent.Decision.CallSupport);

				//	}else{
				//		enemyEntity.ChangeDecision (AIComponent.Decision.Escape);
				//	}
				//	return;
				//}
			} else {
				if (AiToInput.FriendInSight (enemyEntity.m_entity) == true) {
					enemyEntity.ChangeDecision (AIComponent.Decision.CallSupport);
					return;
				} else {
					enemyEntity.ChangeDecision (AIComponent.Decision.Escape);
					return;
				}
				//float attackInjure = enemyEntity.m_events.DecisionExpectInjure (AIComponent.Decision.Attack);
				//float patrolInjeure = enemyEntity.m_events.DecisionExpectInjure (AIComponent.Decision.Patrol);
				//if (attackInjure < patrolInjeure) {
				//	return;
				//} else {
				//	enemyEntity.ChangeDecision (AIComponent.Decision.CallSupport);
				//	return;
				//}
			}
		}

		if (monitorComp.m_voice.Count == 0) {
			enemyEntity.ChangeDecision (AIComponent.Decision.Patrol);
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
			if ( pos!=mPos &&FindPath.GetPath (mPos, pos) != null) {
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

public class CallSupportDecision<T> : BasicDecision<T> where T : BasicEnemy
{
	int tmpHp;

	public CallSupportDecision ()
	{
		SetDecesionType (AIComponent.Decision.CallSupport);
	}

	public override void Enter (T enemyEntity)
	{

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
			//如果遇到了敌人，且敌人的距离足够近
			double threat = FuzzyLogic.DistanceThreat (CalculateDistance (enemyEntity.m_entity), propertyComp.HP);

			if (threat > 0.4f) {
				enemyEntity.ChangeDecision (AIComponent.Decision.Attack);
				return;
			} else {
				enemyEntity.ChangeDecision (AIComponent.Decision.Escape);
				return;
			}
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
public static class FuzzyLogic
{

	public static double AttackBelong (int nowHealth, int allHealth)
	{
		return FuzzyGrade (nowHealth / allHealth, 0, 0.8);
	}

	public static double ProtectBelong (int nowHealth, int allHealth)
	{
		return FuzzyVerseGrade (nowHealth / allHealth, 0.05, 0.2);
	}

	public static double DistanceThreat (int distance, int view)
	{
		return FuzzyVerseGrade (distance / view, 0.2, 1);
	}
	//直线型隶属函数
	static double FuzzyGrade (double value, double x0, double x1)
	{
		double result = 0;
		double x = value;

		if (x <= x0)
			result = 0;
		else if (x >= x1)
			result = 1;
		else
			result = (x / (x1 - x0)) - (x0 / (x1 - x0));

		return result;
	}
	//直线型隶属函数
	static double FuzzyVerseGrade (double value, double x0, double x1)
	{
		double result = 0;
		double x = value;

		if (x <= x0)
			result = 0;
		else if (x >= x1)
			result = 1;
		else
			result = (x / (x1 - x0)) - (x0 / (x1 - x0));

		return 1 - result;
	}
	//三角形隶属函数
	static double FuzzyTriangle (double value, double x0, double x1, double x2)
	{
		double result = 0;
		double x = value;

		if (x <= x0 || x >= x2)
			result = 0;
		else if (x - x1 < 0.1f)
			result = 1;
		else if ((x > x0) && (x < x1))
			result = (x / (x1 - x0)) - (x0 / (x1 - x0));
		else
			result = (-x / (x2 - x1)) + (x2 / (x2 - x1));

		return result;
	}

	//梯形隶属函数
	static double FuzzyTrapezoid (double value, double x0, double x1, double x2, double x3)
	{
		double result = 0;
		double x = value;

		if (x <= x0 || x >= x3)
			result = 0;
		else if ((x >= x1) && (x <= x2))
			result = 1;
		else if ((x > x0) && (x < x1))
			result = (x / (x1 - x0)) - (x0 / (x1 - x0));
		else
			result = (-x / (x3 - x2)) + (x3 / (x3 - x2));

		return result;
	}

}