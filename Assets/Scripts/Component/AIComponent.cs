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

		{AIComponent.Decision.Patrol, 0.5f},
		{AIComponent.Decision.Survey, 0.5f}
	};
	//静态攻击伤害概率，概率越高，敌人攻击欲望越强
	public static Dictionary<AIComponent.Decision, float> StaticBayesDemage = new Dictionary<AIComponent.Decision, float> (){

		{AIComponent.Decision.CallSupport, 0.5f},

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

public enum EnemyState
{
    Select,     //状态管理什么都不做，等待玩家输入
    Action,     //动画执行时调用，卸载所有的临时组件
    Wait        //回合管理开始执行，选择下一个角色，加入所需的组件
}

public class AIComponent : BasicComponent
{
	public enum Decision
	{
		Null,
		Patrol,     //巡逻
		Survey,     //调查
		Attack,     //攻击
					//Escape,     //逃跑
		CallSupport,//呼叫支援
	}
	//ai的核心
	public BasicEnemy m_enemy;
	public Decision m_currentDecision = Decision.Null;
	public List<Vector3> m_patrolPoint = new List<Vector3> ();

    public EnemyState m_enemyState = EnemyState.Wait;
    public float m_actionInterval=1;
    public float m_coldTime=0;


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