using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

static public class FindPath
{
    public class NodeItem
    {
        // 是否是墙
        public bool isWall;

        // 格子坐标
        public Vector3 m_logPos;

        // 与起点的长度
        public int gCost;
        // 与目标点的长度
        public int hCost;

        // 总的路径长度
        public int fCost
        {
            get { return gCost + hCost; }
        }

        // 父节点
        public NodeItem parent;

        public NodeItem(bool isWall, Vector3 logPos)
        {
            this.isWall = isWall;
            m_logPos = logPos;
        }
    }

    //获取从s到e的路径，返回路径长度小于等于step
    static public Vector3 GetPathByStep(Vector3 s, Vector3 e ,int step)
    {
        VoxelBlocks map = GameObject.Find("Voxel Map").GetComponentInChildren<VoxelBlocks>();
        List<Vector3> path= null;

        BlockInfo block = map.GetBlockByLogicPos(e);
        if (block.entity != null)
        {
            BlockType blockType = block.entity.GetComponent<BlockInfoComponent>().m_blockType;
            block.entity.GetComponent<BlockInfoComponent>().m_blockType = BlockType.None;
            path = GetPath(s, e);
            block.entity.GetComponent<BlockInfoComponent>().m_blockType = blockType;
            path.Remove(path[path.Count - 1]);
        }
        else
        {
            path = GetPath(s, e);
        }
        if (path!=null && path.Count > step)
        {
            path.RemoveRange(step , path.Count - step);
            
        }
        
        if(path!=null&&path.Count>0)
            return path[path.Count-1];
        return Vector3.down;
    }

    static public Vector3 GetNearestFriend(Vector3 s)
    {
        List<BasicEntity> enemyList = StateStaticComponent.enemyActionList;
        int num = -1;
        int min = -1;
        for(int i =0;i< enemyList.Count;i++)
        {
            BasicEntity e = enemyList[i];
            Vector3 pos = e.GetComponent<BlockInfoComponent>().m_logicPosition;
            e.GetComponent<BlockInfoComponent>().m_blockType = BlockType.None;
            List<Vector3> path = FindPath.GetPath(s, pos);
            e.GetComponent<BlockInfoComponent>().m_blockType = BlockType.Enemy;
            if (path != null&&path.Count>0) {
                if (i == 0)
                {
                    num = 0;
                    min = path.Count;
                }
                if (min > path.Count)
                {
                    min = path.Count;
                    num = i;
                }
            }
        }
        if (num >= 0)
            return enemyList[num].GetComponent<BlockInfoComponent>().m_logicPosition;
        return Vector3.down;
    }

    static public List<Vector3> GetPath(Vector3 s,Vector3 e) {
        List<NodeItem> openSet = new List<NodeItem>();
        List<NodeItem> closeSet = new List<NodeItem>();
        VoxelBlocks voxelBlocks = GameObject.Find("Voxel Map").GetComponentInChildren<VoxelBlocks>();

        NodeItem startNode = BItoNI(s);
        NodeItem endNode = BItoNI(e);
        openSet.Add(startNode);
        while (openSet.Count > 0)
        {
            NodeItem curNode = openSet[0];

            //获取openSet内权重最低的点。
            for (int i = 0, max = openSet.Count; i < max; i++)
            {
                if (openSet[i].fCost <= curNode.fCost &&
                    openSet[i].hCost < curNode.hCost)
                {
                    curNode = openSet[i];
                }
            }
            openSet.Remove(curNode);
            closeSet.Add(curNode);

            // 找到的目标节点
            if (curNode.m_logPos == e)
            {
                return generatePath(curNode);
            }


            // 判断周围节点，选择一个最优的节点
            foreach (var block in voxelBlocks.GetNeibor(curNode.m_logPos))
            {
                NodeItem item=BItoNI(block.logPos);
                // 如果是墙或者已经在关闭列表中

                if (item.isWall || ContainsNode(closeSet, item))
                {
                    continue;
                }
                // 计算当前相领节点现开始节点距离
                int newCost = curNode.gCost + getDistanceNodes(curNode, item);
                // 如果距离更小，或者原来不在开始列表中
                if (newCost < item.gCost || !ContainsNode(openSet,item))
                {
                    if (!ContainsNode(openSet, item))
                    {
                        // 更新与开始节点的距离
                        item.gCost = newCost;
                        // 更新与终点的距离
                        item.hCost = getDistanceNodes(item, endNode);
                        // 更新父节点为当前选定的节点
                        item.parent = curNode;

                        openSet.Add(item);
                    }
                    else
                    {
                        foreach(var ni in openSet)
                        {
                            Debug.Log(111);
                            if (ni.m_logPos == item.m_logPos)
                            {
                                ni.gCost = newCost;
                                ni.hCost = getDistanceNodes(item, endNode);
                                ni.parent = curNode;
                            }
                        }
                    }
                }
            }
        }

        return null;
    }

    static private bool ContainsNode(List<NodeItem> ln, NodeItem node)
    {
        foreach(NodeItem ni in ln)
        {
            if (ni.m_logPos == node.m_logPos)
            {
                return true;
            }
        }
        return false;
    }

    static public List<Vector3> FindPathInStep(Vector3 s,int n)
    {
        VoxelBlocks voxelBlocks = GameObject.Find("Voxel Map").GetComponent<VoxelBlocks>();
        List<Vector3> openSet = new List<Vector3>();
        List<Vector3> nextSet = new List<Vector3>();
        List<Vector3> closeSet = new List<Vector3>();
        openSet.Add(s);
        closeSet.Add(openSet[0]);
        //步数限制
        for (int i = 0; i < n; i++)
        {
            //遍历第i步的所有格子
            for (int j = 0; j < openSet.Count; j++)
            {
                foreach (var blocks in voxelBlocks.GetNeibor(openSet[j]))
                {
                    if (blocks.entity != null||closeSet.Contains(blocks.logPos))
                        continue;
                    //将下一步能达到的格子加入到总表
                    closeSet.Add(blocks.logPos);
                    //将下一步能到达的格子单独保存
                    nextSet.Add(blocks.logPos);

                }
            }
            openSet.Clear();
          
            //取出下一步能达到的格子
            if(nextSet.Count>0)
                openSet.AddRange(nextSet);
            nextSet.Clear();
        }
        if (closeSet.Count != 0)
        {
            closeSet.Remove(s);
            return closeSet;
        }
        return null;
    }

    static public List<Vector3> GetArea(Vector3 pos, int ridus)
    {
        List<Vector3> area = new List<Vector3>();
        VoxelBlocks map = GameObject.Find("Voxel Map").GetComponent<VoxelBlocks>();
        Vector3 nPos=pos;
        for (int i = -ridus; i < ridus; i++)
        {
            
            nPos.x = pos.x+i;
            for (int j = -ridus; j < ridus; j++)
            {
                nPos.y = pos.y+j;
                if ((nPos - pos).magnitude >= ridus)
                    continue;
                if (map.GetBlockByLogicPos(nPos) != null)
                    area.Add(nPos);
            }
        }

        return area;
    }

    static private bool checkWall(BlockInfo blockInfo)
    {
        VoxelBlocks voxelBlocks = GameObject.Find("Voxel Map").GetComponent<VoxelBlocks>();
        //获取下方方块的逻辑位置
        Vector3 logPos = blockInfo.logPos;
        logPos.z--;
        //若该位置没有东西就判断其下方的方块
        if (blockInfo.entity == null)
        {
            BasicEntity entity = voxelBlocks.GetBlockByLogicPos(logPos).entity;

            if (entity == null)
            {
                Debug.Log(logPos);
                return true;
            }
            if(entity.GetComponent<BlockInfoComponent>().m_blockType != BlockType.Floor)
                return true;
            return false;
        }
        
        if (blockInfo.entity.GetComponent<BlockInfoComponent>().m_blockType!=BlockType.None)
            return true;
        return false;
    }
    
    static private NodeItem BItoNI(Vector3 pos)
    {
        VoxelBlocks voxelBlocks = GameObject.Find("Voxel Map").GetComponent<VoxelBlocks>();
        NodeItem node;
        BlockInfo block = voxelBlocks.GetBlockByLogicPos(pos);
        if (block == null)
        {
            node = new NodeItem(false, pos);
        }else 
            node = new NodeItem(checkWall(block),block.logPos);
        return node;
    }

    static int getDistanceNodes(NodeItem a, NodeItem b)
    {
        int cntX = (int)Mathf.Abs(a.m_logPos.x - b.m_logPos.x);
        int cntY = (int)Mathf.Abs(a.m_logPos.y - b.m_logPos.y);
        // 判断到底是那个轴相差的距离更远
        if (cntX > cntY)
        {
            return 14 * cntY + 10 * (cntX - cntY);
        }
        else
        {
            return 14 * cntX + 10 * (cntY - cntX);
        }
    }

    static List<Vector3> generatePath(NodeItem start)
    {
        List<Vector3> list = new List<Vector3>();
        VoxelBlocks voxelBlocks = GameObject.Find("Voxel Map").GetComponentInChildren<VoxelBlocks>();
        
        while (start.parent != null)
        {
            list.Add(start.m_logPos);
            start = start.parent;
        }
        list.Reverse();
        return list;
    }
}
