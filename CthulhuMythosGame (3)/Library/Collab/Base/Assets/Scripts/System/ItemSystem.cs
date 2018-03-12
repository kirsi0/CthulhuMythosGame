﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSystem : BasicSystem
{
    public override void Execute(List<BasicEntity> entities)
    {
        foreach(var entity in entities)
       {
            InputComponent input = entity.GetComponent<InputComponent>();
            if (input.leftButtonDown)
            {
                GetItem(entity, input.currentPos);
            }
            if (entity.GetComponent<InputComponent>().currentKey != (int)InputType.UseItem)
                continue;
            ItemType item = entity.GetComponent<ItemComponent>().current;
            switch (item)
            {
                case ItemType.Bottle:
                    Bottle(entity,5,3);
                    break;
                default:
                    break;
            }
        }
    }

    bool GetItem(BasicEntity entity , Vector3 mousePos)
    {
        VoxelBlocks map = GameObject.Find("Voxel Map").transform.GetComponent<VoxelBlocks>(); 
        //获取角色位置
        Vector3 pos = entity.GetComponent<BlockInfoComponent>().m_logicPosition;
        //判断鼠标位置是否在角色旁边
        if ((Mathf.Abs(pos.x - mousePos.x) <= 1.5f) && (Mathf.Abs(pos.y - mousePos.y) <= 1.5f))
        {
            //获取鼠标位置上的格子信息
            BasicEntity itemEntity = map.GetBlockByLogicPos(mousePos).entity;
            
            if (itemEntity != null)
            {
                ItemInfoComponent itemInfo = itemEntity.GetComponent<ItemInfoComponent>();
                //若该格子上不存在物品则退出
                if (itemInfo == null)
                    return false;
                ItemComponent itemComp = entity.GetComponent<ItemComponent>();
                //判断物品是否超出持有上限
                if (itemInfo.num + itemComp.item.Count > itemComp.numLimit)
                {
                    //删除物品
                    itemComp.item.RemoveRange(0, itemInfo.num + itemComp.item.Count - itemComp.numLimit);
                }
                //添加物品
                for(int i = 0; i < itemInfo.num; i++)
                {
                    itemComp.item.Add(itemInfo.type);
                }
                map.RemoveBlock(mousePos);
                itemInfo.m_entity.RemoveAllComponent();
                GameObject.Destroy(itemInfo.gameObject);
            }
            return true;
        }
        else
        {
            return false;
        }
    }

    //entity为扔瓶子的物体，mr为可扔的最远距离,ridus为瓶子影响范围
    void Bottle(BasicEntity entity,float mr,float ridus)
    {
        InputComponent input = entity.GetComponent<InputComponent>();
        VoxelBlocks map = GameObject.Find("Voxel Map").GetComponent<VoxelBlocks>();

        //获取鼠标位置与角色位置的距离
        float r =(input.currentPos-entity.GetComponent<BlockInfoComponent>().m_logicPosition).magnitude;
        //若距离大于预设值则退出
        if (r > mr)
            return;
        //获取瓶子投掷后可能影响的范围
        List<Vector3> ar = FindPath.GetArea(input.currentPos, Mathf.FloorToInt(ridus));
        //显示瓶子影响范围
        List<MonitorComponent> mc = new List<MonitorComponent>();

        UISimple ui = GameObject.Find("UI").GetComponent<UISimple>();
        ui.ShowUI(ar, 3);

        //获取该范围内所有的敌人的监视脚本
        foreach (var pos in ar)
        {
            BlockInfo block = map.GetBlockByLogicPos(pos);
            if (block != null && block.entity != null)
            {
                if (block.entity.GetComponent<BlockInfoComponent>().m_blockType == BlockType.Enemy)
                {
                    mc.Add(block.entity.GetComponent<MonitorComponent>());
                }
            }
        }
        //显示可被影响的敌人

        //若按下鼠标左键则扔出瓶子，影响周围敌人。
        if (input.leftButtonDown)
        {
            Debug.Log(mc.Count);
            foreach(var m in mc)
            {
                if (m != null)
                {
                    m.m_voice.Add(input.currentPos);
                }
            }
            ItemComponent itemComp = entity.GetComponent<ItemComponent>();
            //移除瓶子
            itemComp.item.Remove(ItemType.Bottle);
            //将背包中中第一个物品放在物品栏里
            if (itemComp.item.Count > 0)
            {
                itemComp.current = itemComp.item[0];
            }
            else
            {
                //背包内无东西设为空
                itemComp.current = ItemType.Null;
            }
            //使用一次消耗一点行动点
            StateComponent state = entity.GetComponent<StateComponent>();
            state.m_actionPoint -= 1;
            StateStaticComponent.m_currentSystemState = StateStaticComponent.SystemState.Action;
            state.Invoke("AnimationEnd", 1);
        }
    }
}
