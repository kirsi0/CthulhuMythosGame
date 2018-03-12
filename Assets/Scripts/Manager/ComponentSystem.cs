using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Skyunion;


public class ComponentManager : TSingleton<ComponentManager>

{
	Dictionary<ComponentType, List<BasicEntity>> allComponentTypeEntites;


	private ComponentManager ()
	{
		allComponentTypeEntites = new Dictionary<ComponentType, List<BasicEntity>> ();
	}

	//获取包含特定的组件类型的所有实体
	public List<BasicEntity> GetSpecialEntity (ComponentType componentType)
	{
		if (allComponentTypeEntites.ContainsKey (componentType)) {
			return allComponentTypeEntites [componentType];

		} else {
			return new List<BasicEntity> ();
		}
	}
	//将类型和所对的组件注册
	public void RegisterEntity (ComponentType componentType, BasicEntity entity)
	{
		if (!allComponentTypeEntites.ContainsKey (componentType)) {
			//Debug.Log ("Is creating " + componentType + " Type in ComponentManager");

			allComponentTypeEntites.Add (componentType, new List<BasicEntity> ());

		}
		bool isExit = false;
		foreach (BasicEntity comp in allComponentTypeEntites [componentType]) {
			if (comp == entity) {
				Debug.Log (comp.gameObject.name + " entity is exited!");
				isExit = true;
			}
		}
		if (!isExit) {
			allComponentTypeEntites [componentType].Add (entity);
		}
	}
	//将某一些实体登出
	public void LogoutEntity (ComponentType componentType, BasicEntity entity)
	{
		if (allComponentTypeEntites.ContainsKey (componentType)) {

			for (int i = 0; i < allComponentTypeEntites [componentType].Count; i++) {
				if (allComponentTypeEntites [componentType] [i] == entity) {
					//Debug.Log ("Remove " + componentType + " : " + allComponentTypeEntites [componentType] [i].name + " from component manager");

					allComponentTypeEntites [componentType].RemoveAt (i);

				}
			}

		}
		Debug.Log ("allComponents[componentType].Count" + allComponentTypeEntites [componentType].Count);
		if (allComponentTypeEntites [componentType].Count == 0) {
			allComponentTypeEntites.Remove (componentType);
		}
	}
}

