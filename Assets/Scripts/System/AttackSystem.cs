using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSystem : BasicSystem
{
	private ComponentType m_linkedType = ComponentType.Attack;

	//覆盖父类的m_linkedType
	public override ComponentType M_LinkedType {
		set {
			m_linkedType = value;
		}
		get {
			return m_linkedType;
		}
	}

	//每帧都会调用对应的实体
	public override void Execute (List<BasicEntity> entities)
	{
		foreach (BasicEntity e in entities) {
			AttackComponent attack = e.GetComponent<AttackComponent> ();
			VoxelBlocks map = GameObject.Find ("Voxel Map").transform.GetComponent<VoxelBlocks> ();
			AbilityComponent ab = e.GetComponent<AbilityComponent> ();
			InputComponent input = e.GetComponent<InputComponent> ();
			StateComponent ap = e.GetComponent<StateComponent> ();

			int i = AiToInput.GetAbilityCount(e, M_LinkedType);
            //检测是否按下攻击键

            if (i >= ab.m_temporaryAbility.Count || i  != input.currentKey) {
				continue;
			}

			//若无攻击对象则获取周围可攻击对象
			if (attack.enemy == null) {
				attack.enemy = GetEnemyAround (e);
				if (attack.enemy == null)
					return;
			}
			List<Vector3> el = new List<Vector3> ();
			foreach (var enemy in attack.enemy) {
				el.Add (enemy.GetComponent<BlockInfoComponent> ().m_logicPosition);
			}
			UISimple ui = GameObject.Find ("UI").GetComponent<UISimple> ();
			ui.ShowUI (el, 2);


			//左键攻击
			if (input.leftButtonDown) {
                BasicEntity enemy = map.GetBlockByLogicPos(input.currentPos).entity;
                
				//检测当前选中敌人是否处于攻击范围内
				List<BasicEntity> list = GetEnemyAround (e);
				if (list != null && !list.Contains (enemy)) {
					attack.enemy = list;
					return;
				}
				//扣除敌人HP值
				DeadComponent dead = enemy.GetComponent<DeadComponent> ();
				StateComponent state = e.GetComponent<StateComponent> ();
				dead.hp -= attack.STR;
				state.m_actionPoint -= 1;
				state.Invoke ("AnimationEnd", 1);
				StateStaticComponent.m_currentSystemState = StateStaticComponent.SystemState.Action;
				//播放攻击动画
				//播放敌人受击动画
				//减少AP
			}
		}
	}

	List<BasicEntity> GetEnemyAround (BasicEntity entity)
	{
		VoxelBlocks map = GameObject.Find ("Voxel Map").transform.GetComponent<VoxelBlocks> ();
		List<BlockInfo> list = map.GetNeibor (entity.GetComponent<BlockInfoComponent> ().m_logicPosition);
		List<BasicEntity> lb = new List<BasicEntity> ();


		foreach (var b in list) {
            if (b.entity == null)
                continue;
            BlockType blockType = b.entity.GetComponent<BlockInfoComponent>().m_blockType;
            if (blockType != BlockType.Enemy && blockType != BlockType.Player)
                continue ;
            if (b.entity != null && blockType != entity.GetComponent<BlockInfoComponent> ().m_blockType)
				lb.Add (b.entity);
		}
		if (lb.Count == 0)
			return null;
		return lb;
	}
}