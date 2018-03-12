using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BasicEntity : MonoBehaviour
{
	//所链接的实体游戏对象
	//public GameObject m_GameObject;
	public List<BasicComponent> m_components;

	public virtual void Init ()
	{
		//m_GameObject = gameObject;
		m_components = new List<BasicComponent> ();
		GetComponents<BasicComponent> (m_components);
		//Debug.Log (m_components);
		//如果没有组件已经挂在上面则不需要操作
		if (m_components.Count == 0) {
			return;
		}
		foreach (BasicComponent comp in m_components) {
			//Debug.Log ("save " + comp.m_componentType + " type to ComponentManager");
			ComponentManager.Instance.RegisterEntity (comp.m_componentType, this);
		}
	}

	//添加某一类型的组件
	public BasicComponent AddComponent (ComponentType type)
	{
		if (ExistSpecialComponent (type)) {
            //Debug.Log("Exist same component Type" + type);
            return GetSpecicalComponent (type);

		}
		//Debug.Log ("Add component" + type + " in " + gameObject);

		PropertyComponent property = GetComponent<PropertyComponent> ();

		switch (type) {

		case ComponentType.Property:
			PropertyComponent propertyComp = gameObject.AddComponent<PropertyComponent> ();
			propertyComp.Init (ComponentType.Property, this);
			m_components.Add (propertyComp);
			return propertyComp;

		case ComponentType.Dead:
			DeadComponent deadComp = gameObject.AddComponent<DeadComponent> ();
			deadComp.Init (ComponentType.Dead, this);
			deadComp.hp = property.HP;
			m_components.Add (deadComp);

			return deadComp;

		case ComponentType.State:
			StateComponent stateComp = gameObject.AddComponent<StateComponent> ();
			stateComp.Init (ComponentType.State, this);
			m_components.Add (stateComp);
			stateComp.m_actionPoint = property.AP;
			return stateComp;

		case ComponentType.Ability:
			AbilityComponent abilityComp = gameObject.AddComponent<AbilityComponent> ();
			abilityComp.Init (ComponentType.Ability, this);
			m_components.Add (abilityComp);
			return abilityComp;

		case ComponentType.Hide:
			HideComponent hideComp = gameObject.AddComponent<HideComponent> ();
			hideComp.Init (ComponentType.Hide, this);
			m_components.Add (hideComp);
			return hideComp;

		case ComponentType.Move:
			MoveComponent moveComp = gameObject.AddComponent<MoveComponent> ();
			moveComp.Init (ComponentType.Move, this);
			m_components.Add (moveComp);
			moveComp.moveSpeed = property.moveSpd;
			moveComp.SPD = property.SPD;
			return moveComp;

		case ComponentType.Attack:
			AttackComponent attackComp = gameObject.AddComponent<AttackComponent> ();
			attackComp.Init (ComponentType.Attack, this);
			m_components.Add (attackComp);
			attackComp.STR = property.STR;
			return attackComp;

		case ComponentType.Input:
			InputComponent inputComp = gameObject.AddComponent<InputComponent> ();
			inputComp.Init (ComponentType.Input, this);
			m_components.Add (inputComp);
			return inputComp;

		case ComponentType.CheerUp:
			CheerUpComponent cheerUpComp = gameObject.AddComponent<CheerUpComponent> ();
			cheerUpComp.Init (ComponentType.CheerUp, this);
			m_components.Add (cheerUpComp);
			return cheerUpComp;

		case ComponentType.Monitor:
			MonitorComponent monitorComp = gameObject.AddComponent<MonitorComponent> ();
			monitorComp.Init (ComponentType.Monitor, this);
			m_components.Add (monitorComp);
			monitorComp.m_SightArea = property.HRZ;
			return monitorComp;

		case ComponentType.Knock:
			KnockComponent knockComp = gameObject.AddComponent<KnockComponent> ();
			knockComp.Init (ComponentType.Knock, this);
			m_components.Add (knockComp);
			knockComp.m_ridus = property.HRZ;
			return knockComp;

		case ComponentType.Item:
			ItemComponent itemComp = gameObject.AddComponent<ItemComponent> ();
			itemComp.Init (ComponentType.Item, this);
            itemComp.item=property.item;
            itemComp.numLimit = property.itemLimit;
			m_components.Add (itemComp);
			return itemComp;

		case ComponentType.AI:
			AIComponent aiComp = gameObject.AddComponent<AIComponent> ();
			aiComp.Init (ComponentType.AI, this);
			m_components.Add (aiComp);
			return aiComp;
            case ComponentType.UI:
                UIComponent uiComp = gameObject.AddComponent<UIComponent>();
                uiComp.Init(ComponentType.UI, this);
                m_components.Add(uiComp);
                return uiComp;
		default:
			return null;
		}

	}
	//移除某个组件
	public void RemoveComponent (ComponentType type)
	{
		if (m_components.Count == 0) {
			//Debug.Log ("there is no component to delete");
			return;
		}
		//Debug.Log ("Before remove component number is:" + m_components.Count);
		for (int i = 0; i < m_components.Count; i++) {
			//Debug.Log ("e.m_componentType" + m_components [i].m_componentType);
			if (m_components [i].m_componentType == type) {
				ComponentManager.Instance.LogoutEntity (m_components [i].m_componentType, this);
				Destroy (GetSpecicalComponent (type));
				m_components.RemoveAt (i);
				//Debug.Log ("delete " + type + "in " + this.gameObject.name + " success");
				return;
			}
		}
		//Debug.Log ("remove fail: cant find" + type + " component in entity!");
	}
	public void RemoveAllComponent ()
	{
		for (int i = 0; i < m_components.Count; i++) {
			ComponentManager.Instance.LogoutEntity (m_components [i].m_componentType, this);
			Destroy (GetSpecicalComponent (m_components [i].m_componentType));

		}
		m_components.Clear ();
	}
	//获取特定的组件
	public BasicComponent GetSpecicalComponent (ComponentType type)
	{
		foreach (BasicComponent e in m_components) {
			//Debug.Log ("get " + e.m_componentType + " component");

			if (e.m_componentType == type) {
				return e;
			}
		}
		//Debug.Log ("cant find component in entity!");
		return null;
	}

	//是否存在某个组件
	public bool ExistSpecialComponent (ComponentType type)
	{
		foreach (BasicComponent e in m_components) {
			//Debug.Log (e.m_componentType);

			if (e.m_componentType == type) {
				return true;
			}
		}
		return false;
	}

}
