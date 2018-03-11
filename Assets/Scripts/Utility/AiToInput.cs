using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static public class AiToInput
{
	static public bool Attack (BasicEntity entity, Vector3 pos)
	{
		InputComponent input = entity.GetComponent<InputComponent> ();
		if (input == null)
			return false;
		int key = GetAbilityCount (entity, ComponentType.Attack);
		input.currentKey = key;
		input.currentPos = pos;
		input.leftButtonDown = true;
		input.rightButtonDown = true;
		return true;
	}
	static public bool Move (BasicEntity entity, Vector3 pos)
	{
		InputComponent input = entity.GetComponent<InputComponent> ();
		StateComponent state = entity.GetComponent<StateComponent> ();
		MoveComponent move = entity.GetComponent<MoveComponent> ();
		if (input == null)
			return false;
		Vector3 nPos = FindPath.GetPathByStep (entity.GetComponent<BlockInfoComponent> ().m_logicPosition, pos, move.SPD * state.m_actionPoint);
		int key = GetAbilityCount (entity, ComponentType.Move);
		input.currentKey = key;
		input.currentPos = nPos;
		input.leftButtonDown = true;
		return true;
	}
	static public bool CallFriend (BasicEntity entity, List<BasicEntity> player)
	{
		if (player.Count == 0 || entity == null)
			return false;
		MonitorComponent monitor = entity.GetComponent<MonitorComponent> ();
		List<Vector3> view = monitor.m_view;
		VoxelBlocks map = GameObject.Find ("Voxel Map").GetComponent<VoxelBlocks> ();
		bool isFind = false;


		foreach (Vector3 pos in view) {
			BasicEntity e = map.GetBlockByLogicPos (pos).entity;
			if (e != null && e.GetComponent<BlockInfoComponent> ().m_blockType == BlockType.Enemy) {
				isFind = true;
				e.GetComponent<MonitorComponent> ().m_enemy.AddRange (player);
			}
		}
		return isFind;
	}
	static public bool FriendInSight (BasicEntity entity)
	{
		MonitorComponent monitor = entity.GetComponent<MonitorComponent> ();
		foreach (BasicEntity e in StateStaticComponent.enemyActionList) {
			if (monitor.m_view.Contains (e.GetComponent<BlockInfoComponent> ().m_logicPosition))
				return true;
		}
		return false;
	}

	static public int GetAbilityCount (BasicEntity entity, ComponentType type)
	{
		AbilityComponent ability = entity.GetComponent<AbilityComponent> ();
		if (ability == null || ability.m_coldDown.ContainsKey (type))
			return -1;
		int i;
		for (i = 0; i < ability.m_temporaryAbility.Count; i++)
			if (ability.m_temporaryAbility [i] == type)
				return i + 1;
		return -1;
	}

	static public bool ExistEnemy ()
	{
		if (StateStaticComponent.enemyActionList.Count == 1) {
			return false;
		} else {
			return true;
		}

	}

}
