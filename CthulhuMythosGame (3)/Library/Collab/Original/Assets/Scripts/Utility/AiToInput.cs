using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static public class AiToInput {
    static public bool Attack(BasicEntity entity,Vector3 pos)
    {
        InputComponent input = entity.GetComponent<InputComponent>();
        if (input == null)
            return false;
        int key = GetAbilityCount(entity, ComponentType.Attack);
        input.currentKey = key;
        input.currentPos = pos;
        input.leftButtonDown = true;
        return true;
    }
    static public bool Move(BasicEntity entity ,Vector3 pos)
    {
        InputComponent input = entity.GetComponent<InputComponent>();
        if (input == null)
            return false;
        int key = GetAbilityCount(entity, ComponentType.Move);
        input.currentKey = key;
        input.currentPos = pos;
        input.leftButtonDown = true;
        return true;
    }
    static public bool CallFriend(BasicEntity entity)
    {
        MonitorComponent monitor = entity.GetComponent<MonitorComponent>();
        List<BasicEntity> player = monitor.m_enemy;

        if (player.Count == 0||entity==null)
            return false;

        List<Vector3> view = monitor.m_view;
        VoxelBlocks map = GameObject.Find("Voxel Map").GetComponent<VoxelBlocks>();
        StateComponent state = entity.GetComponent<StateComponent>();
        bool isFind = false;
        

        foreach (Vector3 pos in view)
        {
            BasicEntity e = map.GetBlockByLogicPos(pos).entity;
            if (e != null && e.GetComponent<BlockInfoComponent>().m_blockType == BlockType.Enemy)
            {
                isFind = true;
                e.GetComponent<MonitorComponent>().m_enemy.AddRange(player);
            }
        }
        state.m_actionPoint -= 1;
        StateStaticComponent.m_currentSystemState = StateStaticComponent.SystemState.Action;
        state.Invoke("AnimationEnd", 1);
        return isFind;
    }
    static public int GetAbilityCount(BasicEntity entity,ComponentType type)
    {
        AbilityComponent ability = entity.GetComponent<AbilityComponent>();
        if (ability == null)
            return 0;
        int i;
        for (i = 0; i < ability.m_temporaryAbility.Count; i++)
            if (ability.m_temporaryAbility[i] == type)
                return i+1;
        return 0;
    }
}
