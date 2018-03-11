using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
											   //AIComponent.Decision.Escape,
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
		m_currentDecision.Exit (this);

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

		//case AIComponent.Decision.Escape:
		//return new EscapeDecision<Heretic> ();

		case AIComponent.Decision.Patrol:
			return new PatrolDecision<Heretic> ();

		case AIComponent.Decision.Survey:
			return new SurveyDecision<Heretic> ();

		default:
			return null;
		}

	}
}
