using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static class StateStaticComponent
{
	public enum SystemState
	{
		Select,     //状态管理什么都不做，等待玩家输入
		Action,     //动画执行时调用，卸载所有的临时组件
		Wait        //回合管理开始执行，选择下一个角色，加入所需的组件
	}
	public static float afkLimiteTime = 2;
	public static SystemState m_currentSystemState = SystemState.Wait;
	public static BasicEntity m_currentEntity = null;
<<<<<<< HEAD
    public static SystemState m_EcurrentSystemState = SystemState.Wait;
    public static BasicEntity m_EcurrentEntity = null;
	public static int m_turneNumber = 0;
    public static int m_EturnNumber = 0;
=======
	public static int m_turneNumber = 0;
>>>>>>> temp
	public static List<BasicEntity> enemyActionList;
	public static List<BasicEntity> playerActionList;
}

public class StateSystem : BasicSystem
{

	public enum Party
	{
		Player,
		Enemy,
	}

	List<BasicEntity> playerActionList;
	List<BasicEntity> enemyActionList;

	Party currentparty = Party.Player;

	private ComponentType m_linkedType = ComponentType.State;

	//覆盖父类的m_linkedType
	public override ComponentType M_LinkedType {
		set {
			m_linkedType = value;
		}
		get {
			return m_linkedType;
		}
	}

	public override void Init (List<BasicEntity> entities)
	{
		base.Init (entities);
		playerActionList = new List<BasicEntity> ();
		enemyActionList = new List<BasicEntity> ();

		foreach (BasicEntity e in entities) {
			AbilityManager abilityManager = new AbilityManager ();
			//注册长期性的组件
			abilityManager.InitCommomAbility (e);
			BlockType type = e.GetComponent<BlockInfoComponent> ().m_blockType;
			switch (type) {
			case BlockType.Enemy:
				abilityManager.InitEnemyCommomAbility (e);
				break;
			case BlockType.Player:
				abilityManager.InitPlayerCommomAbility (e);
				break;
			default:
				Debug.Log ("please set currect blockType type in " + e.gameObject.name);
				break;
			}
			//添加长期组件
			abilityManager.AddResidentComponent (e);

			PropertyComponent property = (PropertyComponent)e.GetSpecicalComponent (ComponentType.Property);
			//向能力列表中增加不同的能力组件
			switch (property.m_characterType) {
			case PropertyComponent.CharacterType.Veteran:
				abilityManager.AddVeteranAbility (e);
				break;

			case PropertyComponent.CharacterType.Hacker:
				abilityManager.AddHackerAbility (e);
				break;

			case PropertyComponent.CharacterType.Drone:
				abilityManager.AddDroneAbility (e);
				break;

			case PropertyComponent.CharacterType.Heretic:
				abilityManager.AddHereticAbility (e);
				break;

			case PropertyComponent.CharacterType.Deepone:
				abilityManager.AddDeeponeAbility (e);
				break;

			default:
				Debug.Log ("please set currect character type in " + e.gameObject.name);
				break;
			}
		}
	}

	public override void Execute (List<BasicEntity> entities)
	{
<<<<<<< HEAD
        if(StateStaticComponent.m_currentEntity != null &&StateStaticComponent.m_currentEntity.GetComponent<InputComponent>() != null 
            && StateStaticComponent.m_currentEntity.GetComponent<InputComponent>().currentKey == (int)InputType.NextOne)
        {
            AbilityManager abilityManager = new AbilityManager();
            abilityManager.RemoveTemppraryComponent(StateStaticComponent.m_currentEntity);
            StateStaticComponent.m_currentEntity = NextOne(playerActionList, StateStaticComponent.m_currentEntity);
            abilityManager.AddTemporaryComponent(StateStaticComponent.m_currentEntity);
        }
        if (StateStaticComponent.m_currentSystemState == StateStaticComponent.SystemState.Action) {
=======
		if (StateStaticComponent.m_currentSystemState == StateStaticComponent.SystemState.Action) {
>>>>>>> temp
			//Debug.Log (StateStaticComponent.m_currentEntity + " aciont is going");
			AbilityManager abilityManager = new AbilityManager ();
			abilityManager.RemoveTemppraryComponent (StateStaticComponent.m_currentEntity);
			StateStaticComponent.m_currentSystemState = StateStaticComponent.SystemState.Select;
		}

<<<<<<< HEAD
        //只有在系统结束工作之后才会重新开始执行
        if (StateStaticComponent.m_currentSystemState == StateStaticComponent.SystemState.Wait ){
=======
		//只有在系统结束工作之后才会重新开始执行
		if (StateStaticComponent.m_currentSystemState == StateStaticComponent.SystemState.Wait) {
>>>>>>> temp
			StateStaticComponent.m_turneNumber++;
			//Debug.Log ("New check is going");
			//如果现在的角色已经死亡或者是行动点耗尽则出队列
			//RemoveDead ();
			//重新排序行动队列
<<<<<<< HEAD
			InitPlayerActionList (entities);

			//选择当前可以行动的角色
			UpdateCurrentEntity (playerActionList);
=======
			InitActionList (entities);

			//选择当前可以行动的角色
			UpdateCurrentEntity ();
>>>>>>> temp

			StateComponent state = (StateComponent)StateStaticComponent.m_currentEntity.GetSpecicalComponent (ComponentType.State);
			AbilityManager abilityManager = new AbilityManager ();

			if (state.m_actionPoint != 0) {
				//增加输入组件

				//根据能力表新建新的组件
				abilityManager.AddTemporaryComponent (StateStaticComponent.m_currentEntity);
			}
			//切换状态
			StateStaticComponent.m_currentSystemState = StateStaticComponent.SystemState.Select;
		}
<<<<<<< HEAD
    }
=======


	}
>>>>>>> temp
	/*
	private void RemoveDead ()
	{
		StateComponent state = (StateComponent)StateStaticComponent.m_currentEntity.GetSpecicalComponent (ComponentType.State);

		if (state.m_healthyPoint <= 0) {
			if (state.m_roleType == StateComponent.RoleType.Player) {
				playerActionList.Remove (StateStaticComponent.m_currentEntity);
			} else if (state.m_roleType == StateComponent.RoleType.Enemy) {
				enemyActionList.Remove (StateStaticComponent.m_currentEntity);
			}
		}
	}
*/
<<<<<<< HEAD
    private BasicEntity NextOne(List<BasicEntity> entitis,BasicEntity entity)
    {
        int i = 0;
        for (i = 0; i < entitis.Count; i++)
        {
            if (entitis[i] == entity)
                break;
        }
        if (i == entitis.Count)
            return entity;
        if (i == entitis.Count - 1)
            i = -1;
        return entitis[i + 1];
    }
    private void InitPlayerActionList(List<BasicEntity> entities)
    {
        playerActionList.Clear();
        //获取实体
        foreach (BasicEntity e in entities)
        {
            DeadComponent deadComp = (DeadComponent)e.GetSpecicalComponent(ComponentType.Dead);
            StateComponent stateComponent = (StateComponent)e.GetSpecicalComponent(ComponentType.State);
            if (deadComp != null && deadComp.hp <= 0)
            {
                continue;
            }
            PropertyComponent PropertyComponent = (PropertyComponent)e.GetSpecicalComponent(ComponentType.Property);

            if (PropertyComponent.m_characterType == PropertyComponent.CharacterType.Veteran ||
                PropertyComponent.m_characterType == PropertyComponent.CharacterType.Hacker ||
                PropertyComponent.m_characterType == PropertyComponent.CharacterType.Drone)
            {
                //将实体入列
                playerActionList.Add(e);
            }
        }
        if (playerActionList.Count == 0)
        {
            Skyunion.UIManager.Instance().ShowPanel<UIGameFail>();
        }
        //TODO：后续修改
        playerActionList.Sort(delegate (BasicEntity x, BasicEntity y) {
            PropertyComponent stateX = (PropertyComponent)x.GetSpecicalComponent(ComponentType.Property);
            PropertyComponent stateY = (PropertyComponent)y.GetSpecicalComponent(ComponentType.Property);
            Debug.Log("stateX.m_agility : " + stateX.m_agility + "stateY.m_agility" + stateY.m_agility);
            return -stateX.m_agility.CompareTo(stateY.m_agility);
        });

        foreach (BasicEntity e in playerActionList)
        {
            Debug.Log(e.gameObject.name + "->");

        }
        StateStaticComponent.playerActionList = playerActionList;
        //确定现在在行动的角色1.是否又有角色在行动。2，角色的行动点是否已经耗尽

    }
    private void InitEnemyActionList(List<BasicEntity> entities)
    {
        enemyActionList.Clear();
        foreach (BasicEntity e in entities)
        {
            DeadComponent deadComp = (DeadComponent)e.GetSpecicalComponent(ComponentType.Dead);
            StateComponent stateComponent = (StateComponent)e.GetSpecicalComponent(ComponentType.State);
            if (deadComp != null && deadComp.hp <= 0)
            {
                continue;
            }
            PropertyComponent PropertyComponent = (PropertyComponent)e.GetSpecicalComponent(ComponentType.Property);

            if (PropertyComponent.m_characterType == PropertyComponent.CharacterType.Heretic ||
                     PropertyComponent.m_characterType == PropertyComponent.CharacterType.Deepone)
            {
                enemyActionList.Add(e);
            }
        }
        if (enemyActionList.Count == 0)
        {
            Skyunion.UIManager.Instance().ShowPanel<UIGameComplete>();
        }
        //TODO：后续修改
        enemyActionList.Sort(delegate (BasicEntity x, BasicEntity y) {
            PropertyComponent stateX = (PropertyComponent)x.GetSpecicalComponent(ComponentType.Property);
            PropertyComponent stateY = (PropertyComponent)y.GetSpecicalComponent(ComponentType.Property);
            Debug.Log("stateX.m_agility : " + stateX.m_agility + "stateY.m_agility" + stateY.m_agility);
            return -stateX.m_agility.CompareTo(stateY.m_agility);
        });

        foreach (BasicEntity e in enemyActionList)
        {
            Debug.Log(e.gameObject.name + "->");

        }
        StateStaticComponent.enemyActionList = enemyActionList; 
        //确定现在在行动的角色1.是否又有角色在行动。2，角色的行动点是否已经耗尽
    }
   
=======
	private void InitActionList (List<BasicEntity> entities)
	{

		enemyActionList.Clear ();
		playerActionList.Clear ();
		//获取实体
		foreach (BasicEntity e in entities) {
			DeadComponent deadComp = (DeadComponent)e.GetSpecicalComponent (ComponentType.Dead);
			StateComponent stateComponent = (StateComponent)e.GetSpecicalComponent (ComponentType.State);
			if (deadComp != null && deadComp.hp <= 0) {
				continue;
			}
			PropertyComponent PropertyComponent = (PropertyComponent)e.GetSpecicalComponent (ComponentType.Property);

			if (PropertyComponent.m_characterType == PropertyComponent.CharacterType.Veteran ||
				PropertyComponent.m_characterType == PropertyComponent.CharacterType.Hacker ||
				PropertyComponent.m_characterType == PropertyComponent.CharacterType.Drone) {
				//将实体入列
				playerActionList.Add (e);
			} else if (PropertyComponent.m_characterType == PropertyComponent.CharacterType.Heretic ||
					   PropertyComponent.m_characterType == PropertyComponent.CharacterType.Deepone) {
				enemyActionList.Add (e);
			}
		}
		if (enemyActionList.Count == 0) {
			Skyunion.UIManager.Instance ().ShowPanel<UIGameCompletePanel> ();
		}
		if (playerActionList.Count == 0) {
			Skyunion.UIManager.Instance ().ShowPanel<UIGameFailPanel> ();
		}
		//TODO：后续修改
		enemyActionList.Sort (delegate (BasicEntity x, BasicEntity y) {
			PropertyComponent stateX = (PropertyComponent)x.GetSpecicalComponent (ComponentType.Property);
			PropertyComponent stateY = (PropertyComponent)y.GetSpecicalComponent (ComponentType.Property);
			Debug.Log ("stateX.m_agility : " + stateX.m_agility + "stateY.m_agility" + stateY.m_agility);
			return -stateX.m_agility.CompareTo (stateY.m_agility);
		});

		playerActionList.Sort (delegate (BasicEntity x, BasicEntity y) {
			PropertyComponent stateX = (PropertyComponent)x.GetSpecicalComponent (ComponentType.Property);
			PropertyComponent stateY = (PropertyComponent)y.GetSpecicalComponent (ComponentType.Property);
			Debug.Log ("stateX.m_agility : " + stateX.m_agility + "stateY.m_agility" + stateY.m_agility);
			return -stateX.m_agility.CompareTo (stateY.m_agility);
		});

		foreach (BasicEntity e in playerActionList) {
			Debug.Log (e.gameObject.name + "->");

		}
		foreach (BasicEntity e in enemyActionList) {
			Debug.Log (e.gameObject.name + "->");

		}
		StateStaticComponent.enemyActionList = enemyActionList;
		StateStaticComponent.playerActionList = playerActionList;
		//确定现在在行动的角色1.是否又有角色在行动。2，角色的行动点是否已经耗尽

	}
>>>>>>> temp
	/*
	private void InitActionPoint ()
	{
		//如果所有的心动数值状态都是0，那么就重置行动点
		bool needReset = true;
		foreach (BasicEntity e in playerActionList) {
			StateComponent state = (StateComponent)e.GetSpecicalComponent (ComponentType.State);
			if (state.m_actionPoint > 0) {
				needReset = false;
			}
		}
		foreach (BasicEntity e in enemyActionList) {
			StateComponent state = (StateComponent)e.GetSpecicalComponent (ComponentType.State);
			if (state.m_actionPoint > 0) {
				needReset = false;
			}
		}
		//todo:需要根据不同的规则计算行动点
		if (needReset) {
			foreach (BasicEntity e in playerActionList) {
				StateComponent state = (StateComponent)e.GetSpecicalComponent (ComponentType.State);
				state.m_actionPoint = 5;

			}
			foreach (BasicEntity e in enemyActionList) {
				StateComponent state = (StateComponent)e.GetSpecicalComponent (ComponentType.State);
				state.m_actionPoint = 5;
			}
		}
	}
	*/

	private void InitActionPoint (List<BasicEntity> actionList)
	{
		foreach (BasicEntity e in actionList) {
			StateComponent state = (StateComponent)e.GetSpecicalComponent (ComponentType.State);
			AbilityComponent ability = (AbilityComponent)e.GetSpecicalComponent (ComponentType.Ability);
			state.m_actionPoint = e.GetComponent<PropertyComponent> ().AP;
			//减少CD
			foreach (ComponentType ct in ability.m_temporaryAbility) {
				if (ability.m_coldDown.ContainsKey (ct)) {
					ability.m_coldDown [ct]--;
					if (ability.m_coldDown [ct] < 0)
						ability.m_coldDown.Remove (ct);
				}
			}
		}
	}
<<<<<<< HEAD
    private void UpdateCurrentEntity(List<BasicEntity> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            StateComponent state = (StateComponent)list[i].GetSpecicalComponent(ComponentType.State);
            if (state.m_actionPoint != 0)
            {
                if (state.GetComponent<BlockInfoComponent>().m_blockType == BlockType.Player)
                    StateStaticComponent.m_currentEntity = list[i];
                else
                    StateStaticComponent.m_EcurrentEntity = list[i];
                return;
            }
        }
        //重新填充行动点 
        InitActionPoint(list);
        //重新调用
        UpdateCurrentEntity(list);
        return;
    }
	
=======
	private void UpdateCurrentEntity ()
	{
		//if (playerActionList.Count == 0 && enemyActionList.Count == 0) {
		//	Debug.Log ("Error, the num of character is 0");
		//	return;
		//}
		//如果当前的是玩家行动
		if (currentparty == Party.Player) {
			for (int i = 0; i < playerActionList.Count; i++) {
				StateComponent state = (StateComponent)playerActionList [i].GetSpecicalComponent (ComponentType.State);
				if (state.m_actionPoint != 0) {
					currentparty = Party.Player;
					StateStaticComponent.m_currentEntity = playerActionList [i];
					return;
				}
			}
			//如果没有找到合适的角色，那么就换到敌人回合行动
			currentparty = Party.Enemy;
			//重新填充行动点 
			InitActionPoint (playerActionList);
			//重新调用
			UpdateCurrentEntity ();
			return;
		}

		if (currentparty == Party.Enemy) {
			for (int i = 0; i < enemyActionList.Count; i++) {
				StateComponent state = (StateComponent)enemyActionList [i].GetSpecicalComponent (ComponentType.State);
				if (state.m_actionPoint != 0) {
					currentparty = Party.Enemy;
					StateStaticComponent.m_currentEntity = enemyActionList [i];
					return;
				}
			}
			currentparty = Party.Player;
			InitActionPoint (enemyActionList);
			UpdateCurrentEntity ();
			return;
		}
	}
>>>>>>> temp


}
