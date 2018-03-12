using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Skyunion;
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
    public List<Vector3> m_patrolPoint=new List<Vector3>();
}

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
	}
	public abstract void Execute ();
	public abstract void ChangeDecision (AIComponent.Decision newDecision);
}

public class Heretic : BasicEnemy
{
	public int STR;
	public int HP;
	public int San;
	public int AP;
	public int HRZ;

	//当前所处的决策
	public BasicDecision<Heretic> m_currentDecision;
	//之前所处的决策
	public BasicDecision<Heretic> m_previsousDecsion;
	//实际的决策实体所在的位置
	Dictionary<AIComponent.Decision, BasicDecision<Heretic>> m_decisions;
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

		foreach (AIComponent.Decision decision in containDecision) {
			m_decisions.Add (decision, CreateDecision (decision));
		}

		m_currentDecision = m_decisions [AIComponent.Decision.Patrol];
		m_previsousDecsion = m_currentDecision;
	}

	public override void Execute ()
	{
		m_currentDecision.Execte (this);
	}

	public override void ChangeDecision (AIComponent.Decision newDecision)
	{
		m_previsousDecsion = m_currentDecision;
		m_previsousDecsion.Exit (this);
		m_currentDecision = m_decisions [newDecision];
		m_currentDecision.Enter (this);
	}

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
	//造成伤害的决定权重
	const int demageWeight = 1;
	//受到伤害的决定权重
	const int ingureWeight = 1;

	Event [] m_events;
	int m_currentEvent;

	DecisionEvent ()
	{
		m_events = new Event [eventLength];
		m_currentEvent = 0;
	}
	public void AddEvent (AIComponent.Decision decision)
	{
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

	public float DecisionExpectDemage (AIComponent.Decision decision)
	{
		return ExpectDemage (decision) * demageWeight * BayesDemage (decision);
	}

	public float ExpectDemage (AIComponent.Decision decision)
	{
		int demageTime = 0;
		float expectDemage = 0;
		foreach (Event e in m_events) {
			if (e.m_decision == decision) {
				expectDemage += e.m_demage;
				demageTime++;
			}
		}
		if (demageTime == 0) {
			return 0;
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
			return 0;
		}
		float denominator = allDemage / allExcute;
		return (executeDemage / decisionExecute) * (decisionExecute / allExcute);
	}

	public float DecisionExpectInjure (AIComponent.Decision decision)
	{
		return ExpectInjure (decision) * ingureWeight * BayesInjure (decision);
	}

	public float ExpectInjure (AIComponent.Decision decision)
	{
		int ingureTime = 0;
		float expectInjure = 0;
		foreach (Event e in m_events) {
			if (e.m_decision == decision) {
				expectInjure += e.m_injure;
				ingureTime++;
			}
		}
		if (ingureTime == 0) {
			return 0;
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
			return 0;
		}
		float denominator = allInjure / allExcute;
		return (executeInjure / decisionExecute) * (decisionExecute / allExcute);
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
	int tmpHp = 0;

	public PatrolDecision ()
	{
		SetDecesionType (AIComponent.Decision.Patrol);
	}

	public override void Enter (T enemyEntity)
	{
		enemyEntity.m_events.AddEvent (AIComponent.Decision.Patrol);
		DeadComponent deadComp = (DeadComponent)enemyEntity.m_entity.GetSpecicalComponent (ComponentType.Dead);
		tmpHp = deadComp.hp;
		Patrol (enemyEntity.m_entity);
		return;

	}

	public override void Execte (T enemyEntity)
	{
		MonitorComponent monitorComp = (MonitorComponent)enemyEntity.m_entity.GetSpecicalComponent (ComponentType.Monitor);
		PropertyComponent propertyComp = (PropertyComponent)enemyEntity.m_entity.GetSpecicalComponent (ComponentType.Property);
		DeadComponent deadComp = (DeadComponent)enemyEntity.m_entity.GetSpecicalComponent (ComponentType.Dead);

		if (monitorComp.m_enemy.Count != 0) {
			//贝叶斯算法，确定损耗之后再做决定
			//生命值高且
			double degreeAttack = FuzzyLogic.AttackBelong (deadComp.hp, propertyComp.HP);
			double degreeProtect = FuzzyLogic.ProtectBelong (deadComp.hp, propertyComp.HP);
			if (degreeAttack >= degreeProtect) {
				float attackDemage = enemyEntity.m_events.DecisionExpectDemage (AIComponent.Decision.Attack);
				float patrolDemage = enemyEntity.m_events.DecisionExpectDemage (AIComponent.Decision.Patrol);
				if (attackDemage > patrolDemage) {
					enemyEntity.ChangeDecision (AIComponent.Decision.Attack);
				}
			} else {
				float attackInjure = enemyEntity.m_events.DecisionExpectInjure (AIComponent.Decision.Attack);
				float patrolInjeure = enemyEntity.m_events.DecisionExpectInjure (AIComponent.Decision.Patrol);
				if (attackInjure < patrolInjeure) {
					enemyEntity.ChangeDecision (AIComponent.Decision.Attack);
				}
			}
		}
		if (monitorComp.m_view.Count != 0) {
			double degreeAttack = FuzzyLogic.AttackBelong (deadComp.hp, propertyComp.HP);
			double degreeProtect = FuzzyLogic.ProtectBelong (deadComp.hp, propertyComp.HP);
			if (degreeAttack >= degreeProtect) {
				float surveyDemage = enemyEntity.m_events.DecisionExpectDemage (AIComponent.Decision.Survey);
				float patrolDemage = enemyEntity.m_events.DecisionExpectDemage (AIComponent.Decision.Patrol);
				if (surveyDemage > patrolDemage) {
					enemyEntity.ChangeDecision (AIComponent.Decision.Survey);
				}
			} else {
				float surveyInjure = enemyEntity.m_events.DecisionExpectInjure (AIComponent.Decision.Survey);
				float patrolInjeure = enemyEntity.m_events.DecisionExpectInjure (AIComponent.Decision.Patrol);
				if (surveyInjure < patrolInjeure) {
					enemyEntity.ChangeDecision (AIComponent.Decision.Survey);
				}
			}
		}
		//计算受伤
		enemyEntity.m_events.AddInjure (tmpHp - deadComp.hp);
		tmpHp = deadComp.hp;

		Patrol (enemyEntity.m_entity);
		return;
	}
	public void Patrol (BasicEntity entity)
	{
        //寻找一个坐标进行来回移动
        AIComponent ai = entity.GetComponent<AIComponent>();
        StateComponent state = entity.GetComponent<StateComponent>();
        MoveComponent move = entity.GetComponent<MoveComponent>();

        List<Vector3> path = ai.m_patrolPoint;
        Vector3 entityPos = entity.GetComponent<BlockInfoComponent>().m_logicPosition;
        int i;
        for (i = 0; i < path.Count; i++)
        {
            if (path[i] == entityPos)
                break;
        }
        i++;
        if (i ==path.Count+1)
        {
            VoxelBlocks map = GameObject.Find("Voxel Map").transform.GetComponent<VoxelBlocks>();
            BlockInfo blockInfo = map.GetBlockByLogicPos(path[0]);
            if (blockInfo.entity == null)
            {
                AiToInput.Move(entity,path[0]);
            }else
            {
                AiToInput.Move(entity, path[1]);
            }
        }
        if (i == path.Count)
            i = 0;
        AiToInput.Move(entity, path[i]);
	}

	public override void Exit (T enemyEntity)
	{
		DeadComponent deadComp = (DeadComponent)enemyEntity.m_entity.GetSpecicalComponent (ComponentType.Dead);

		enemyEntity.m_events.AddInjure (tmpHp - deadComp.hp);
		tmpHp = deadComp.hp;
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
		MonitorComponent monitorComp = (MonitorComponent)enemyEntity.m_entity.GetSpecicalComponent (ComponentType.Monitor);
		target = monitorComp.m_enemy [0];
		DeadComponent selfDeadComp = (DeadComponent)enemyEntity.m_entity.GetSpecicalComponent (ComponentType.Dead);
		DeadComponent targetDeadComp = (DeadComponent)target.GetSpecicalComponent (ComponentType.Dead);
		tmpHp = selfDeadComp.hp;
		tmpTargetHp = targetDeadComp.hp;
		enemyEntity.m_events.AddEvent (AIComponent.Decision.Attack);
		Attack (enemyEntity.m_entity);
		return;

	}

	public override void Execte (T enemyEntity)
	{

		MonitorComponent monitorComp = (MonitorComponent)enemyEntity.m_entity.GetSpecicalComponent (ComponentType.Monitor);
		PropertyComponent propertyComp = (PropertyComponent)enemyEntity.m_entity.GetSpecicalComponent (ComponentType.Property);
		DeadComponent deadComp = (DeadComponent)enemyEntity.m_entity.GetSpecicalComponent (ComponentType.Dead);

		//生命值不足，进入逃跑
		if (deadComp.hp < 0.1f * propertyComp.HP) {
			enemyEntity.ChangeDecision (AIComponent.Decision.Escape);
		}

		//如果敌人已经死亡
		if (target == null) {

			if (monitorComp.m_enemy.Count != 0) {
				target = monitorComp.m_enemy [0];
			}

			if (monitorComp.m_view.Count != 0) {
				double degreeAttack = FuzzyLogic.AttackBelong (deadComp.hp, propertyComp.HP);
				double degreeProtect = FuzzyLogic.ProtectBelong (deadComp.hp, propertyComp.HP);
				if (degreeAttack >= degreeProtect) {
					float surveyDemage = enemyEntity.m_events.DecisionExpectDemage (AIComponent.Decision.Survey);
					float patrolDemage = enemyEntity.m_events.DecisionExpectDemage (AIComponent.Decision.Patrol);
					if (surveyDemage > patrolDemage) {
						enemyEntity.ChangeDecision (AIComponent.Decision.Survey);
					}
				} else {
					float surveyInjure = enemyEntity.m_events.DecisionExpectInjure (AIComponent.Decision.Survey);
					float patrolInjeure = enemyEntity.m_events.DecisionExpectInjure (AIComponent.Decision.Patrol);
					if (surveyInjure < patrolInjeure) {
						enemyEntity.ChangeDecision (AIComponent.Decision.Survey);
					}
				}
			}
			//返回巡逻
			enemyEntity.ChangeDecision (AIComponent.Decision.Patrol);
		}

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
        VoxelBlocks map = GameObject.Find("Voxel Map").transform.GetComponent<VoxelBlocks>();
        Vector3 ePos = entity.GetComponent<BlockInfoComponent>().m_logicPosition;
        Vector3 tPos = target.GetComponent<BlockInfoComponent>().m_logicPosition;
        if(Mathf.Abs(ePos.x - tPos.x) <= 1 && Mathf.Abs(ePos.y - tPos.y) <= 1)
        {
            AiToInput.Attack(entity, tPos);
        }
        else
        {
            target.GetComponent<BlockInfoComponent>().m_blockType = BlockType.None;
            List<Vector3> path = FindPath.GetPath(ePos, tPos);
            target.GetComponent<BlockInfoComponent>().m_blockType = BlockType.Player;
            if (path != null)
            {
                path.Remove(tPos);
                tPos = path[path.Count - 1];
                AiToInput.Move(entity, tPos);
            }
            else
            {
                Debug.Log("Fail to Attack");
            }
        }

    }

    public override void Exit (T enemyEntity)
	{

		DeadComponent selfDeadComp = (DeadComponent)enemyEntity.m_entity.GetSpecicalComponent (ComponentType.Dead);
		DeadComponent targetDeadComp = (DeadComponent)target.GetSpecicalComponent (ComponentType.Dead);

		enemyEntity.m_events.AddInjure (tmpHp - selfDeadComp.hp);
		enemyEntity.m_events.AddDemage (tmpTargetHp - targetDeadComp.hp);
		tmpHp = selfDeadComp.hp;
		tmpTargetHp = targetDeadComp.hp;
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
		Escape (enemyEntity.m_entity);
		return;

	}

	public override void Execte (T enemyEntity)
	{
		MonitorComponent monitorComp = (MonitorComponent)enemyEntity.m_entity.GetSpecicalComponent (ComponentType.Monitor);
		DeadComponent deadComp = (DeadComponent)enemyEntity.m_entity.GetSpecicalComponent (ComponentType.Dead);

		if (monitorComp.m_enemy.Count == 0 && monitorComp.m_view.Count == 0) {
			enemyEntity.ChangeDecision (AIComponent.Decision.Patrol);
		}

		//计算受伤
		enemyEntity.m_events.AddInjure (tmpHp - deadComp.hp);
		tmpHp = deadComp.hp;

		Escape (enemyEntity.m_entity);
		return;
	}

	public void Escape (BasicEntity entity)
	{
        MonitorComponent monitorComp = entity.GetComponent<MonitorComponent>();
        List<Vector3> voice = monitorComp.m_voice;
        List<BasicEntity> player = monitorComp.m_enemy;
        Vector3 mPos = entity.GetComponent<BlockInfoComponent>().m_logicPosition;
        Vector3 escapePos=Vector3.zero;

        if (voice.Count > 0)
        {
            foreach(Vector3 vPos in voice)
            {
                Vector3 pos = mPos - vPos;
                escapePos += (pos / pos.magnitude/3);
            }
        }
        if (player.Count > 0)
        {
            foreach(BasicEntity role in player)
            {
                Vector3 pos =mPos - role.GetComponent<BlockInfoComponent>().m_logicPosition;
                escapePos += (pos / pos.magnitude);
            }
        }
        if (escapePos != Vector3.zero)
        {
            escapePos.Normalize();

            StateComponent ap = entity.GetComponent<StateComponent>();
            MonitorComponent monitor = entity.GetComponent<MonitorComponent>();
            MoveComponent move = entity.GetComponent<MoveComponent>();
            Vector3 pos=mPos;
            for (int i = move.SPD; i >0; i--)
            {
                pos.x = Mathf.FloorToInt(escapePos.x * i) + mPos.x;
                pos.y = Mathf.FloorToInt(escapePos.y * i) + mPos.y;
                List<Vector3> path = FindPath.GetPath(mPos, pos);
                if (path != null)
                {
                    AiToInput.Move(entity, pos);
                }
            }
        }
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
		enemyEntity.m_events.AddEvent (AIComponent.Decision.Survey);
		DeadComponent deadComp = (DeadComponent)enemyEntity.m_entity.GetSpecicalComponent (ComponentType.Dead);
		tmpHp = deadComp.hp;
		Survey (enemyEntity.m_entity);
		return;
	}

	public override void Execte (T enemyEntity)
	{
		MonitorComponent monitorComp = (MonitorComponent)enemyEntity.m_entity.GetSpecicalComponent (ComponentType.Monitor);
		PropertyComponent propertyComp = (PropertyComponent)enemyEntity.m_entity.GetSpecicalComponent (ComponentType.Property);
		DeadComponent deadComp = (DeadComponent)enemyEntity.m_entity.GetSpecicalComponent (ComponentType.Dead);

		if (monitorComp.m_enemy.Count != 0) {
			//贝叶斯算法，确定损耗之后再做决定
			//生命值高且
			double degreeAttack = FuzzyLogic.AttackBelong (deadComp.hp, propertyComp.HP);
			double degreeProtect = FuzzyLogic.ProtectBelong (deadComp.hp, propertyComp.HP);
			if (degreeAttack >= degreeProtect) {
				float attackDemage = enemyEntity.m_events.DecisionExpectDemage (AIComponent.Decision.Attack);
				float patrolDemage = enemyEntity.m_events.DecisionExpectDemage (AIComponent.Decision.Patrol);
				if (attackDemage > patrolDemage) {
					enemyEntity.ChangeDecision (AIComponent.Decision.Attack);
				} else {
					enemyEntity.ChangeDecision (AIComponent.Decision.CallSupport);
				}
			} else {
				float attackInjure = enemyEntity.m_events.DecisionExpectInjure (AIComponent.Decision.Attack);
				float patrolInjeure = enemyEntity.m_events.DecisionExpectInjure (AIComponent.Decision.Patrol);
				if (attackInjure < patrolInjeure) {
					enemyEntity.ChangeDecision (AIComponent.Decision.Attack);
				} else {
					enemyEntity.ChangeDecision (AIComponent.Decision.CallSupport);
				}
			}
		} else {
			enemyEntity.ChangeDecision (AIComponent.Decision.Patrol);
		}

		//计算受伤
		enemyEntity.m_events.AddInjure (tmpHp - deadComp.hp);
		tmpHp = deadComp.hp;

		Survey (enemyEntity.m_entity);
		return;
	}

	public void Survey (BasicEntity entity)
	{
        VoxelBlocks map = GameObject.Find("Voxel Map").transform.GetComponent<VoxelBlocks>();

        List<Vector3> voice = entity.GetComponent<MonitorComponent>().m_voice;
        Vector3 mPos = entity.GetComponent<BlockInfoComponent>().m_logicPosition;
        List<Vector3> newVoice = new List<Vector3>();
        foreach(var pos in voice)
        {
            BasicEntity e = map.GetBlockByLogicPos(pos).entity;
            //若该位置有物体则找离物体最近的路
            if (e != null)
            {
                e.GetComponent<BlockInfoComponent>().m_blockType = BlockType.None;
                Vector3 tPos = e.GetComponent<BlockInfoComponent>().m_logicPosition;
                List<Vector3> path = FindPath.GetPath(mPos, tPos);
                e.GetComponent<BlockInfoComponent>().m_blockType = BlockType.Player;
                if (path != null)
                {
                    path.Remove(tPos);
                    tPos = path[path.Count - 1];
                    newVoice.Add(tPos);
                }
            }
            //若能直接走到该位置则保存，不能就放弃
              else if (FindPath.GetPath(mPos, pos) != null)
            {
                newVoice.Add(pos);
            }
        }

        if (newVoice.Count != 0)
        {
            //若已到达调查位置，转巡逻状态
            if (mPos == newVoice[newVoice.Count - 1])
            {
                ;
            }
            else
            {
                AiToInput.Move(entity, newVoice[newVoice.Count - 1]);
            }
        }
        
	}

	public override void Exit (T enemyEntity)
	{
		DeadComponent deadComp = (DeadComponent)enemyEntity.m_entity.GetSpecicalComponent (ComponentType.Dead);
        enemyEntity.m_entity.GetComponent<MonitorComponent>().m_voice = new List<Vector3>();
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
		CallSupport (enemyEntity.m_entity);
		return;

	}

	public override void Execte (T enemyEntity)
	{
		MonitorComponent monitorComp = (MonitorComponent)enemyEntity.m_entity.GetSpecicalComponent (ComponentType.Monitor);
		PropertyComponent propertyComp = (PropertyComponent)enemyEntity.m_entity.GetSpecicalComponent (ComponentType.Property);
		DeadComponent deadComp = (DeadComponent)enemyEntity.m_entity.GetSpecicalComponent (ComponentType.Dead);


		//计算受伤
		enemyEntity.m_events.AddInjure (tmpHp - deadComp.hp);
		tmpHp = deadComp.hp;

		CallSupport (enemyEntity.m_entity);
		return;
	}

	public void CallSupport (BasicEntity entity)
	{
        AiToInput.CallFriend(entity);
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
		return FuzzyGrade (nowHealth / allHealth, 0.4, 0.8);
	}

	public static double ProtectBelong (int nowHealth, int allHealth)
	{
		return FuzzyGrade (nowHealth / allHealth, 0.2, 0.6);
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