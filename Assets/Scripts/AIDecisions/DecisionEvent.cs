using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
		//m_currentEvent -1 才是真正的当前事件
		m_currentEvent++;
		if (m_currentEvent == eventLength) {
			m_currentEvent = 0;
		}
	}

	public AIComponent.Decision CurrentDecision ()
	{
		return m_events [m_currentEvent - 1].m_decision;
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
		//如果还没有足够的数据，那么从静态参数中取值
		if (allExcute == 0 || allInjure == 0 || decisionExecute == 0) {
			return Probability.StaticBayesInjure [decision];
		}
		float denominator = allInjure / allExcute;
		return ((executeInjure / decisionExecute) * (decisionExecute / allExcute)) / denominator;
	}
}
