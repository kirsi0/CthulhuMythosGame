using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideSystem : BasicSystem{

    public override void Execute(List<BasicEntity> entities)
    {
        foreach(var e in entities)
        {
            //掩体方向
            Vector3 hidePos = FindCover(e.GetComponent<BlockInfoComponent>().m_logicPosition);

            if (hidePos != Vector3.down)
            {
                //执行靠墙动作
            }
            else
            {
                //执行站立动作
            }
        }
    }

    private Vector3 FindCover(Vector3 pos)
    {
        VoxelBlocks map = GameObject.Find("Voxel Map").GetComponent<VoxelBlocks>();


        List<BlockInfo> list = map.GetNeibor(pos);
        if (list == null)
            return Vector3.down;
        foreach (var e in list)
        {

            if (e.entity != null && e.entity.GetComponent<BlockInfoComponent>().m_blockType == BlockType.Wall)
                return e.logPos;
        }

        return Vector3.down;
    }
}
