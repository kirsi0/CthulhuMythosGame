using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Skyunion;

public class SystemManager : MonoSingleton<SystemManager>
{
	//玩家队列
	private List<BasicEntity> PlayerList;
	//敌人队列
	private List<BasicEntity> EnemyList;
	//系统队列
	private List<BasicSystem> basicSystems;

	private void Awake ()
	{


		PlayerList = new List<BasicEntity> ();
		EnemyList = new List<BasicEntity> ();
		basicSystems = new List<BasicSystem> ();

		//找到对应game object动态加载实体

		////动态加载模块
		//AttackComponent attackComponent = GameObject.Find ("HealthyCube").AddComponent<AttackComponent> ();
		////必须执行，依次加载组件的初始化，动态加载的组件会在组件管理器中注册
		//attackComponent.Init (ComponentType.Attack, cube);

		////必须执行，最后初始化实体，它会把直接挂载在game object上的组件一起注册
		//cube.Init ();

		//PlayerList.Add (cube);

		DeadSystem deadSystem = new DeadSystem ();
		deadSystem.M_LinkedType = ComponentType.Dead;

		HideSystem hideSystem = new HideSystem ();
		hideSystem.M_LinkedType = ComponentType.Hide;

		MoveSystem moveSystem = new MoveSystem ();
		moveSystem.M_LinkedType = ComponentType.Move;

		AttackSystem attackSystem = new AttackSystem ();
		attackSystem.M_LinkedType = ComponentType.Attack;

		InputSystem inputSystem = new InputSystem ();
		inputSystem.M_LinkedType = ComponentType.Input;

		UISystem uiSystem = new UISystem ();
		uiSystem.M_LinkedType = ComponentType.UI;

		CheerUpSystem cheerUpSystem = new CheerUpSystem ();
		cheerUpSystem.M_LinkedType = ComponentType.CheerUp;

		MonitorSystem monitorSystem = new MonitorSystem ();
		monitorSystem.M_LinkedType = ComponentType.Monitor;

		KnockSystem knockSystem = new KnockSystem ();
		knockSystem.M_LinkedType = ComponentType.Knock;

		ItemSystem itemSystem = new ItemSystem ();
		itemSystem.M_LinkedType = ComponentType.Item;

		AISystem aiSystem = new AISystem ();
		aiSystem.M_LinkedType = ComponentType.AI;

		basicSystems.Add (deadSystem);
		basicSystems.Add (hideSystem);
		basicSystems.Add (moveSystem);
		basicSystems.Add (attackSystem);
		basicSystems.Add (inputSystem);
		basicSystems.Add (uiSystem);
		basicSystems.Add (cheerUpSystem);
		basicSystems.Add (monitorSystem);
		basicSystems.Add (knockSystem);
		basicSystems.Add (itemSystem);
		basicSystems.Add (aiSystem);

	}


	private void Start ()
	{
		//现在game object中只有property和entity
		GameObject [] objects = GameObject.FindGameObjectsWithTag ("Character");
		foreach (GameObject obj in objects) {
			BasicEntity entity = obj.GetComponent<BasicEntity> ();

			////能力组件是添加其他所有组件的入口
			//AbilityComponent abilityComp = (AbilityComponent)entity.AddComponent (ComponentType.Ability);
			//abilityComp.Init (ComponentType.Ability, entity);

			entity.Init ();
		}

		//初始化系统组件，把现在的所有实体传入
		StateSystem stateSystem = new StateSystem ();
		stateSystem.M_LinkedType = ComponentType.State;
		List<BasicEntity> ent = stateSystem.Fliter (ComponentType.Property);
		//初始化所有需要的组件
		if (ent != null) {
			stateSystem.Init (ent);
		}


		//所有系统的初始化函数
		foreach (BasicSystem basicSystem in basicSystems) {
			Debug.Log ("init " + basicSystem.M_LinkedType + " System");
			List<BasicEntity> entities = basicSystem.Fliter (basicSystem.M_LinkedType);
			if (entities != null) {
				Debug.Log ("execute system:" + basicSystem.M_LinkedType + "system");
				basicSystem.Init (entities);
			}
		}

		//最后才加入系统表，防止二次初始化
		basicSystems.Add (stateSystem);
	}

	private void Update ()
	{
		UISimple ui = GameObject.Find ("UI").GetComponent<UISimple> ();
		ui.CloseUI ();
		foreach (BasicSystem basicSystem in basicSystems) {
			//Debug.Log ("Update " + basicSystem.M_LinkedType + " System");
			List<BasicEntity> entities = basicSystem.Fliter (basicSystem.M_LinkedType);
			if (entities != null) {
				//Debug.Log ("execute system:" + basicSystem.M_LinkedType + "system");
				basicSystem.Execute (entities);
			}
		}

		//回合制

	}

	private BasicSystem GetSystem (ComponentType type)
	{
		foreach (BasicSystem sys in basicSystems) {
			if (sys.M_LinkedType == type) {
				return sys;
			}
		}
		Debug.Log ("Cant Find System Type: " + type);
		return null;
	}
}



