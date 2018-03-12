using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockSystem : BasicSystem {

    public override void Execute(List<BasicEntity> entities)
    {
        foreach(var entity in entities)
        {
            KnockComponent knock = entity.gameObject.GetComponent<KnockComponent>();
            AbilityComponent ab = entity.GetComponent<AbilityComponent>();
            InputComponent input = entity.GetComponent<InputComponent>();
            StateComponent ap = entity.GetComponent<StateComponent>();

            int i = AiToInput.GetAbilityCount(entity,M_LinkedType);
            if (i >= ab.m_temporaryAbility.Count || i != input.currentKey)
            {
                knock.m_area = null;
                continue;
            }
            //获取影响范围
            if (knock.m_area == null) {
                knock.m_area = FindPath.GetArea(knock.GetComponent<BlockInfoComponent>().m_logicPosition, knock.m_ridus);
            }
            UISimple ui = GameObject.Find("UI").GetComponent<UISimple>();
            ui.ShowUI(knock.m_area, 3);
            VoxelBlocks map = GameObject.Find("Voxel Map").GetComponent<VoxelBlocks>();
            List<BasicEntity> enemy=new List<BasicEntity>();
            

            //获取影响范围内的敌人
            foreach (var pos in knock.m_area)
            {
                var e = map.GetBlockByLogicPos(pos).entity;
                if (e != null)
                {
                    if (e.GetComponent<BlockInfoComponent>().m_blockType == BlockType.Enemy)
                    {
                        enemy.Add(e);
                    }
                }
            }
            //UI显示范围与敌人
            if (input.leftButtonDown)
            {
                foreach(var e in enemy)
                {
                    Debug.Log(e.name);
                    e.GetComponent<MonitorComponent>().m_voice.Add(entity.GetComponent<BlockInfoComponent>().m_logicPosition);
                }
                ui.ShowUI(null, 3);
                StateComponent state = entity.GetComponent<StateComponent>();
                state.m_actionPoint -= 1;
                StateStaticComponent.m_currentSystemState = StateStaticComponent.SystemState.Action;
                state.Invoke("AnimationEnd", 1);
            }
        
        }
    }

}
