using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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