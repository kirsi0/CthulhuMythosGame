
using System.Collections.Generic;

public class AbilityManager
{

	//增加临时组件
	public bool AddTemporaryComponent (BasicEntity entity)
	{
		AbilityComponent ability = entity.GetComponent<AbilityComponent> ();
		if (ability == null)
			return false;
		foreach (ComponentType c in ability.m_temporaryAbility) {
			entity.AddComponent (c);
		}
		return true;
	}
	//增加长期组件
	public bool AddResidentComponent (BasicEntity entity)
	{
		AbilityComponent ability = entity.GetComponent<AbilityComponent> ();
		if (ability == null)
			return false;
		foreach (ComponentType c in ability.m_residentAbility) {
			entity.AddComponent (c);
		}
		return true;
	}

	//移除短期组件
	public bool RemoveTemppraryComponent (BasicEntity entity)
	{
		AbilityComponent ability = entity.GetComponent<AbilityComponent> ();
		if (ability == null)
			return false;
		foreach (ComponentType c in ability.m_temporaryAbility) {
			entity.RemoveComponent (c);
		}
		return true;
	}

	public bool AddAbility (BasicEntity entity, ComponentType type)
	{
		AbilityComponent ability = entity.GetComponent<AbilityComponent> ();
		if (ability == null)
			return false;
		ability.m_temporaryAbility.Add (type);
		return true;
	}

	public bool RemoveAbility (BasicEntity entity, ComponentType type)
	{
		AbilityComponent ability = entity.GetComponent<AbilityComponent> ();
		if (ability == null)
			return false;
		ability.m_temporaryAbility.Remove (type);
		return true;
	}

	public void InitAbility (BasicEntity entity)
	{
		PropertyComponent propertyComponent = (PropertyComponent)entity.GetSpecicalComponent (ComponentType.Property);
		if (propertyComponent.m_characterType == PropertyComponent.CharacterType.Null) {
			return;
		}

		switch (propertyComponent.m_characterType) {

		case PropertyComponent.CharacterType.Veteran:
			AddVeteranAbility (entity);
			break;

		case PropertyComponent.CharacterType.Hacker:
			AddHackerAbility (entity);
			break;

		case PropertyComponent.CharacterType.Drone:
			AddDroneAbility (entity);
			break;

		case PropertyComponent.CharacterType.Heretic:
			AddHereticAbility (entity);
			break;

		case PropertyComponent.CharacterType.Deepone:
			AddDeeponeAbility (entity);
			break;

		}
	}
	//通用组件
	public void InitCommomAbility (BasicEntity entity)
	{
		//增加ability组件
		AbilityComponent ability = (AbilityComponent)entity.AddComponent (ComponentType.Ability);

		ability.m_residentAbility.Add (ComponentType.State);
		ability.m_residentAbility.Add (ComponentType.Dead);
		ability.m_residentAbility.Add (ComponentType.Hide);

	}
	//敌人通用组件
	public void InitEnemyCommomAbility (BasicEntity entity)
	{
		AbilityComponent ability = entity.GetComponent<AbilityComponent> ();

		ability.m_residentAbility.Add (ComponentType.Monitor);
		ability.m_residentAbility.Add (ComponentType.AI);
	}

	//玩家通用组件
	public void InitPlayerCommomAbility (BasicEntity entity)
	{
		AbilityComponent ability = entity.GetComponent<AbilityComponent> ();
	}

	public void AddVeteranAbility (BasicEntity entity)
	{
		AbilityComponent ability = entity.GetComponent<AbilityComponent> ();

		ability.m_temporaryAbility.Add (ComponentType.Input);
		ability.m_temporaryAbility.Add (ComponentType.CheerUp);
		ability.m_temporaryAbility.Add (ComponentType.Move);
		ability.m_temporaryAbility.Add (ComponentType.Attack);
		ability.m_temporaryAbility.Add (ComponentType.Knock);
		ability.m_temporaryAbility.Add (ComponentType.Item);
		ability.m_temporaryAbility.Add (ComponentType.UI);
	}

	public void AddHackerAbility (BasicEntity entity)
	{
		AbilityComponent ability = entity.GetComponent<AbilityComponent> ();

		ability.m_temporaryAbility.Add (ComponentType.Input);

		ability.m_temporaryAbility.Add (ComponentType.Drone);
		ability.m_temporaryAbility.Add (ComponentType.Move);
		ability.m_temporaryAbility.Add (ComponentType.Attack);
		ability.m_temporaryAbility.Add (ComponentType.Knock);
		ability.m_temporaryAbility.Add (ComponentType.Item);
		ability.m_temporaryAbility.Add (ComponentType.UI);
	}

	public void AddDroneAbility (BasicEntity entity)
	{
		AbilityComponent ability = entity.GetComponent<AbilityComponent> ();

		ability.m_temporaryAbility.Add (ComponentType.Input);

		ability.m_temporaryAbility.Add (ComponentType.Hack);

		ability.m_temporaryAbility.Add (ComponentType.Move);
		ability.m_temporaryAbility.Add (ComponentType.Attack);
		ability.m_temporaryAbility.Add (ComponentType.UI);
	}

	public void AddHereticAbility (BasicEntity entity)
	{

		AbilityComponent ability = entity.GetComponent<AbilityComponent> ();

		ability.m_temporaryAbility.Add (ComponentType.Input);
		ability.m_temporaryAbility.Add (ComponentType.Move);
		ability.m_temporaryAbility.Add (ComponentType.Attack);
		ability.m_temporaryAbility.Add (ComponentType.UI);
	}

	public void AddDeeponeAbility (BasicEntity entity)
	{

		AbilityComponent ability = entity.GetComponent<AbilityComponent> ();

		ability.m_temporaryAbility.Add (ComponentType.Input);

		ability.m_temporaryAbility.Add (ComponentType.Move);
		ability.m_temporaryAbility.Add (ComponentType.Attack);
		ability.m_temporaryAbility.Add (ComponentType.UI);
	}

    
}


/*
public static class PlayerAbilityList
{
	public class AbilityList
	{
		List<ComponentType> m_abilities;

		public AbilityList (ComponentType [] ablities)
		{
			m_abilities = new List<ComponentType> ();
			foreach (ComponentType a in ablities) {
				m_abilities.Add (a);
			}
		}
	}

	static public AbilityList m_veteran = new AbilityList (new ComponentType []{

		ComponentType.Attack, ComponentType.Attacked, ComponentType.Move,
		ComponentType.Idle,ComponentType.Covered,ComponentType.Dead,

		ComponentType.CheerUp

	});

	static public AbilityList m_hacker = new AbilityList (new ComponentType []{

		ComponentType.Attack, ComponentType.Attacked, ComponentType.Move,
		ComponentType.Idle,ComponentType.Covered,ComponentType.Dead,

		ComponentType.Drone

	});

	static public AbilityList m_heretic = new AbilityList (new ComponentType []{

		ComponentType.Attack, ComponentType.Attacked, ComponentType.Move,
		ComponentType.Idle,ComponentType.Covered,ComponentType.Dead,

		ComponentType.Hacker

	});

	static public AbilityList m_deepone = new AbilityList (new ComponentType []{

		ComponentType.Attack, ComponentType.Attacked, ComponentType.Move,
		ComponentType.Idle,ComponentType.Covered,ComponentType.Dead

	});


}

*/
