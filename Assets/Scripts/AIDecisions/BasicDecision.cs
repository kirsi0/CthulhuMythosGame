using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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


